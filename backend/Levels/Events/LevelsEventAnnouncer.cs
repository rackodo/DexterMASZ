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
				ITextChannel? txtAnnouncementChannel = null;
				if (channel is IVoiceChannel && config.SendVoiceLevelUps)
				{
					if (config.VoiceLevelUpChannel == 0) return;
					txtAnnouncementChannel = (ITextChannel)_client.GetChannel(config.VoiceLevelUpChannel);
				}
				if (channel is ITextChannel c && config.SendTextLevelUps)
				{
					txtAnnouncementChannel = c;
				}

				if (txtAnnouncementChannel is null) return;
				string template = config.LevelUpMessageOverrides.GetValueOrDefault(level, config.LevelUpTemplate);
				string msg = template
					.Replace("{USER}", guildUser.Mention)
					.Replace("{LEVEL}", level.ToString());
				await txtAnnouncementChannel.SendMessageAsync(msg);
			}
			catch (Exception e)
			{
				_logger.LogError(e, $"Error while announcing level up to {channel.Id} for guild {guildUser.GuildId}.");
			}
		}
	}
}
