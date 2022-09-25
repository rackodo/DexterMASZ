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
	}

	public void RegisterEvents()
	{
		_client.MessageReceived += ProcessMessage;
		_botEventHandler.OnBotLaunched += SetupTimer;
		_eventHandler.OnGuildLevelConfigUpdated += HandleGuildConfigChanged;
		_eventHandler.OnGuildLevelConfigCreated += HandleGuildConfigChanged;
		_eventHandler.OnGuildLevelConfigDeleted += HandleGuildConfigDeleted;
		_eventHandler.OnUserLevelUp += HandleLevelRoles;
		_client.UserJoined += HandleUpdateRoles;
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

	public async Task<string> HandleUpdateRoles(IGuildUser user)
	{
		using var scope = _serviceProvider.CreateScope();
		return await HandleUpdateRoles(user, scope);
	}

	public async Task<string> HandleUpdateRoles(IGuildUser user, IServiceScope serviceScope)
	{
		var guildId = user.GuildId;

		var levelConfigRepo = serviceScope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>();
		var guildConfig = await levelConfigRepo.GetOrCreateConfig(guildId);
		if (guildConfig == null) return "Unable to locate guild configuration";

		var userLevel = serviceScope.ServiceProvider.GetRequiredService<GuildUserLevelRepository>().GetLevel(user.Id, guildId);
		if (userLevel == null) return "Unable to locate user's level";

		var calc = new CalculatedGuildUserLevel(userLevel, guildConfig);
		return await HandleLevelRoles(userLevel, calc.Total.Level, user, null, levelConfigRepo);
	}

	public async Task<string> HandleLevelRoles(GuildUserLevel guildUserLevel, int level, IGuildUser guildUser, IChannel? channel)
	{
		using var scope = _serviceProvider.CreateScope();
		var configRepo = scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>();
		return await HandleLevelRoles(guildUserLevel, level, guildUser, channel, configRepo);
	}

	public async Task<string> HandleLevelRoles(GuildUserLevel guildUserLevel, int level, IGuildUser guildUser, IChannel? channel, GuildLevelConfigRepository levelConfigRepo)
	{
		try
		{
			var config = await levelConfigRepo.GetOrCreateConfig(guildUserLevel.GuildId);
			if (!config.HandleRoles)
				return "This guild has leveled role handling disabled!";

			var toAdd = new HashSet<ulong>();
			var toRemove = new HashSet<ulong>();
			var currRoles = guildUser.RoleIds;

			foreach (var entry in config.Levels)
			{
				if (level < entry.Key)
				{
					Array.ForEach(entry.Value, (v) => toRemove.Add(v));
					continue;
				}
				else
				{
					Array.ForEach(entry.Value, (v) => toAdd.Add(v));
				}
			}

			if (config.NicknameDisabledRole != default && currRoles.Contains(config.NicknameDisabledRole) && toAdd.Contains(config.NicknameDisabledReplacement))
				toAdd.Remove(config.NicknameDisabledReplacement);

			var guild = guildUser.Guild;
			var guildRoles = guild.Roles;
			var guildRoleIds = guild.Roles.Select(x => x.Id);

			toAdd.IntersectWith(guildRoleIds);
			toRemove.IntersectWith(guildRoleIds);

			toRemove.IntersectWith(currRoles);
			toAdd.ExceptWith(currRoles);

			Task.WaitAll(guildUser.AddRolesAsync(toAdd), guildUser.RemoveRolesAsync(toRemove));

			var stringify = (IEnumerable<ulong> list) =>
			{
				var cnt = list.Count();
				var result = $"{cnt} role{(cnt == 1 ? "" : "s")}";
				if (cnt > 0)
					result += " (" + string.Join(", ", list.Select(r => guildUser.Guild.GetRole(r).Name)) + ")";
				return result;
			};

			return $"Successfully added {stringify(toAdd)} and removed {stringify(toRemove)} for user {guildUser.Mention} (level {level}).";
		}
		catch (Exception e)
		{
			_logger.LogError(e, $"Error while handling roles on level up for user {guildUser.Id} in guild {guildUser.GuildId}.");
			return "ERROR: " + e.Message;
		}
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
			var guild = _client.GetGuild(cds.guildId);
			if (guild == null)
			{
				var guildName = (await _client.Rest.GetGuildAsync(cds.guildId))?.Name;
				_logger.LogError($"Unable to retrieve guild vc data for guild {guildName ?? "Unknown"} ({cds.guildId})");
				continue;
			}

			foreach (var vchannel in guild.VoiceChannels)
			{
				if (config.DisabledXpChannels.Contains(vchannel.Id)) continue;

				var nonbotusers = 0;
				List<IGuildUser> toLevel = new();
				foreach (IGuildUser uservc in vchannel.ConnectedUsers)
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

				if (nonbotusers < config.VoiceXpRequiredMembers) continue;

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

	private class GuildCooldowns
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
}