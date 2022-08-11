using Bot.Abstractions;
using Bot.Events;
using Bot.Extensions;
using Discord;
using Discord.WebSocket;
using Levels.Data;
using Levels.Events;
using Levels.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Timers;

namespace Levels.Services;

public class LevelingService : Event
{
	private readonly DiscordSocketClient _client;
	private readonly ILogger<LevelingService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly BotEventHandler _botEventHandler;
	private readonly LevelsEventHandler _eventHandler;

	private readonly Random _random;
	private readonly Dictionary<ulong, GuildCooldowns> guildCooldowns = new();

	public LevelingService(DiscordSocketClient client, ILogger<LevelingService> logger, IServiceProvider services, BotEventHandler botEventHandler,
		LevelsEventHandler eventHandler)
	{
		_client = client;
		_logger = logger;
		_serviceProvider = services;
		_botEventHandler = botEventHandler;
		_eventHandler = eventHandler;

		_random = new Random();

		// GET ALL REGISTERED GUILDS' CONFIGS AND INJECT INTO GUILDCOOLDOWNS
	}

	public void RegisterEvents()
	{
		_client.MessageReceived += ProcessMessage;
		_botEventHandler.OnBotLaunched += SetupTimer;
		_eventHandler.OnGuildLevelConfigUpdated += HandleGuildConfigChanged;
		_eventHandler.OnGuildLevelConfigCreated += HandleGuildConfigChanged;
		_eventHandler.OnGuildLevelConfigDeleted += HandleGuildConfigDeleted;
	}

	public async Task GrantXP(int xp, XPType xptype, GuildUserLevel level, GuildLevelConfig config, IGuildUser user, IGuildChannel channel, IServiceScope scope)
	{
		// REQUEST ITEMS FROM THE INVENTORY DATABASE TO RECALCULATE XP
		
		var grantedxp = 0;
		if (xptype.HasFlag(XPType.Text))
		{
			level.TextXp += xp;
			grantedxp += xp;
		} 
		else if (xptype.HasFlag(XPType.Voice))
		{
			level.VoiceXp += xp;
			grantedxp += xp;
		}
		CalculatedGuildUserLevel calcLevel = new(level, config);
		if (calcLevel.Total.Residualxp < grantedxp)
		{
			_eventHandler.UserLevelUpEvent.Invoke(level, calcLevel.Total.Level, user, channel);
		}
		var levelrepo = scope.ServiceProvider.GetRequiredService<GuildUserLevelRepository>();
		await levelrepo.UpdateLevel(level);
	}

	private Task SetupTimer()
	{
		System.Timers.Timer timer = new(5000);
		timer.Elapsed += TimerCallback;
		timer.Start();

		using var scope = _serviceProvider.CreateScope();
		var levelconfigrepo = scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>();
		var configs = levelconfigrepo.GetAllRegistered();
		foreach (var config in configs)
		{
			_logger.LogInformation($"Registering XP Timer for Guild {config.Id}");
			guildCooldowns.Add(config.Id, new GuildCooldowns(config.Id, config.XpInterval));
		}

		return Task.CompletedTask;
	}

	private void TimerCallback(object? source, ElapsedEventArgs e)
	{
		Task.Run(PeriodicCheck);
	}

	private async Task ProcessMessage(IMessage message)
	{
		if (message.Channel is not IGuildChannel guildChannel) 
			return;

		if (message.Author.IsBot)
			return;

		var guild = guildChannel.Guild;
		if (!guildCooldowns.ContainsKey(guild.Id))
			return;

		var gcds = guildCooldowns[guild.Id];
		if (gcds.textUsers.Contains(message.Author.Id))
			return;

		using var scope = _serviceProvider.CreateScope();
		var configrepo = scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>();

		var config = await configrepo.GetOrCreateConfig(guild.Id);
		if (config.DisabledXpChannels.Contains(message.Channel.Id))
			return;

		gcds.textUsers.Add(message.Author.Id);

		var user = await guild.GetUserAsync(message.Author.Id) ??
			await _client.Rest.GetGuildUserAsync(guild.Id, message.Author.Id);

		var levelrepo = scope.ServiceProvider.GetRequiredService<GuildUserLevelRepository>();
		await GrantXP(_random.Next(config.MinimumTextXpGiven, config.MaximumTextXpGiven + 1),
			XPType.Text, await levelrepo.GetOrCreateLevel(user), config, user, guildChannel, scope);
	}

	private async Task PeriodicCheck()
	{
		var timenow = DateTimeOffset.Now.ToUnixTimeSeconds();
		foreach (var cds in guildCooldowns.Values)
		{
			if (timenow < cds.nextRefresh) continue;
			cds.nextRefresh += cds.refreshInterval;
			cds.textUsers.Clear();

			using var scope = _serviceProvider.CreateScope();
			var configrepo = scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>();
			var levelrepo = scope.ServiceProvider.GetRequiredService<GuildUserLevelRepository>();

			var config = await configrepo.GetOrCreateConfig(cds.guildId);
			IGuild guild = _client.GetGuild(cds.guildId);
			if (guild is null) guild = await _client.Rest.GetGuildAsync(cds.guildId);

			foreach (var vchannel in await guild.GetVoiceChannelsAsync())
			{
				if (config.DisabledXpChannels.Contains(vchannel.Id)) continue;

				var nonbotusers = 0;
				List<IGuildUser> toLevel = new();
				await foreach (IGuildUser uservc in vchannel.GetUsersAsync())
				{
					// CHECK IF USER IS RESTRICTED ON VOICE XP

					if (uservc.IsBot || uservc.IsDeafened || uservc.IsSelfDeafened) continue;
					if (uservc.IsMuted || uservc.IsSelfMuted || uservc.IsSuppressed)
					{
						if (config.VoiceXpCountMutedMembers) nonbotusers++;
						continue;
					}
					toLevel.Add(uservc);
					nonbotusers++;
				}

				foreach (var u in toLevel)
				{
					await GrantXP(_random.Next(config.MinimumVoiceXpGiven, config.MaximumVoiceXpGiven + 1),
						XPType.Voice,
						await levelrepo.GetOrCreateLevel(u.GuildId, u.Id),
						config,
						u,
						vchannel,
						scope
						);
				}
			}
		}
	}

	private Task HandleGuildConfigChanged(GuildLevelConfig guildLevelConfig)
	{
		if (!guildCooldowns.ContainsKey(guildLevelConfig.Id))
		{
			guildCooldowns.Add(guildLevelConfig.Id, new GuildCooldowns(guildLevelConfig.Id, guildLevelConfig.XpInterval));
		}
		else
		{
			var gcds = guildCooldowns[guildLevelConfig.Id];
			if (gcds.refreshInterval == guildLevelConfig.XpInterval) return Task.CompletedTask;
			gcds.refreshInterval = guildLevelConfig.XpInterval;
			guildCooldowns[guildLevelConfig.Id].nextRefresh = DateTimeOffset.Now.ToUnixTimeSeconds() + gcds.refreshInterval;
		}
		return Task.CompletedTask;
	}

	private Task HandleGuildConfigDeleted(GuildLevelConfig guildLevelConfig)
	{
		if (guildCooldowns.ContainsKey(guildLevelConfig.Id))
		{
			guildCooldowns.Remove(guildLevelConfig.Id);
		}
		return Task.CompletedTask;
	}
}

class GuildCooldowns
{
	public GuildCooldowns(ulong guildId, int refreshInterval)
	{
		this.guildId = guildId;
		this.refreshInterval = refreshInterval;

		nextRefresh = DateTimeOffset.Now.ToUnixTimeSeconds() + refreshInterval;
		textUsers = new HashSet<ulong>();
	} 

	public ulong guildId;
	public HashSet<ulong> textUsers;
	public long nextRefresh;
	public int refreshInterval;
}
