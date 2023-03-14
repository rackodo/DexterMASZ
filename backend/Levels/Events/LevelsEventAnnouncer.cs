using Bot.Abstractions;
using Discord;
using Discord.WebSocket;
using Levels.Data;
using Levels.Models;
using Levels.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Levels.Events;

public class LevelsEventAnnouncer : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly LevelsEventHandler _eventHandler;
    private readonly LevelingService _levelService;
    private readonly ILogger<LevelsEventAnnouncer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LevelsEventAnnouncer(LevelsEventHandler eventHandler, LevelingService levelService,
        ILogger<LevelsEventAnnouncer> logger,
        IServiceProvider serviceProvider, DiscordSocketClient client)
    {
        _eventHandler = eventHandler;
        _levelService = levelService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = client;
    }

    public void RegisterEvents() => _eventHandler.OnUserLevelUp += AnnounceLevelUp;

    private async Task AnnounceLevelUp(GuildUserLevel guildUserLevel, int level, IGuildUser guildUser,
        IChannel channel)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation(
            $"{guildUser.Username}#{guildUser.Discriminator} ({guildUser.Id}) leveled up in {guildUser.GuildId}/{channel?.Id.ToString() ?? "Unknown Channel"} to level {level}");

        if (channel is not null)
            try
            {
                var config = await scope.ServiceProvider.GetRequiredService<GuildLevelConfigRepository>()
                    .GetOrCreateConfig(guildUserLevel.GuildId);
                IMessageChannel levelUpChannel = null;
                IMessageChannel announcementChannel = null;
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

                foreach (var c in new[] { levelUpChannel, announcementChannel })
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
