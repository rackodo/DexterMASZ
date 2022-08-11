using Bot.Abstractions;
using Discord;
using Discord.WebSocket;
using Levels.Data;
using Levels.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Levels.Events;

public class LevelsEventAnnouncer : Event
{
	private readonly LevelsEventHandler _eventHandler;
	private readonly ILogger<LevelsEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly DiscordSocketClient _client;

	public LevelsEventAnnouncer(LevelsEventHandler eventHandler, ILogger<LevelsEventAnnouncer> logger,
		IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnUserLevelUp += AnnounceLevelUp;
		_eventHandler.OnUserLevelUp += HandleLevelRoles;
	}

	private async Task AnnounceLevelUp(GuildUserLevel guildUserLevel, int level, IGuildUser guildUser, IChannel? channel)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"{guildUser.Username}#{guildUser.Discriminator} ({guildUser.Id}) leveled up in {guildUser.GuildId}/{channel?.Id.ToString() ?? "Unknown Channel"} to level {level}");

		if (channel is not null)
		{
			try
			{
				var config = await scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>().GetOrCreateConfig(guildUserLevel.GuildId);
				IMessageChannel? levelUpChannel = null;
				IMessageChannel? announcementChannel = null;
				if (channel is IVoiceChannel vc)
				{
					if (config.VoiceLevelUpChannel != 0)
						announcementChannel = (IMessageChannel)_client.GetChannel(config.VoiceLevelUpChannel);
					levelUpChannel = config.SendVoiceLevelUps ? vc : null;
				}
				else if (channel is ITextChannel tc)
				{
					if (config.TextLevelUpChannel != 0)
						announcementChannel = (IMessageChannel)_client.GetChannel(config.TextLevelUpChannel);
					levelUpChannel = config.SendTextLevelUps ? tc : null;
				}

				var template = config.LevelUpMessageOverrides.GetValueOrDefault(level, config.LevelUpTemplate);
				if (string.IsNullOrEmpty(template)) return;
				var msg = template
					.Replace("{USER}", guildUser.Mention)
					.Replace("{LEVEL}", level.ToString());
				
				foreach (var c in new IMessageChannel?[] { levelUpChannel, announcementChannel })
				{
					if (c is null) continue;
					await c.SendMessageAsync(msg);
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error while announcing level up to {channel.Id} for guild {guildUser.GuildId}.");
			}
		}
	}

	private async Task HandleLevelRoles(GuildUserLevel guildUserLevel, int level, IGuildUser guildUser, IChannel? channel)
	{
		using var scope = _serviceProvider.CreateScope();

		try
		{
			var config = await scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>().GetOrCreateConfig(guildUserLevel.GuildId);
			if (!config.HandleRoles)
				return;

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
		}
		catch (Exception e)
		{
			_logger.LogError(e, $"Error while handling roles on level up for user {guildUser.Id} in guild {guildUser.GuildId}.");
		}
	}
}
