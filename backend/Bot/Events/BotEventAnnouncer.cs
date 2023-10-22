using Bot.Abstractions;
using Bot.Extensions;
using Bot.Models;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.Events;

public class BotEventAnnouncer(BotEventHandler eventHandler, ILogger<BotEventAnnouncer> logger,
    IServiceProvider serviceProvider, DiscordSocketClient client) : IEvent
{
    private readonly DiscordSocketClient _client = client;
    private readonly BotEventHandler _eventHandler = eventHandler;
    private readonly ILogger<BotEventAnnouncer> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

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
