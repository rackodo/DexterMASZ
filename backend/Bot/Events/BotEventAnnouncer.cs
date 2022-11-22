using Bot.Abstractions;
using Bot.Extensions;
using Bot.Models;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.Events;

public class BotEventAnnouncer : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly BotEventHandler _eventHandler;
    private readonly ILogger<BotEventAnnouncer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public BotEventAnnouncer(BotEventHandler eventHandler, ILogger<BotEventAnnouncer> logger,
        IServiceProvider serviceProvider, DiscordSocketClient client)
    {
        _eventHandler = eventHandler;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = client;
    }

    public void RegisterEvents() => _eventHandler.OnGuildRegistered += async (a, b) => await AnnounceTipsInNewGuild(a);

    private async Task AnnounceTipsInNewGuild(GuildConfig guildConfig)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var embed = await guildConfig.CreateTipsEmbedForNewGuilds(scope.ServiceProvider);

            await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffAnnouncements, embed);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while announcing tips to {guildConfig.StaffAnnouncements} for guild {guildConfig.GuildId}.");
        }
    }
}
