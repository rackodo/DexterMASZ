using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MOTDs.Extensions;
using MOTDs.Models;

namespace MOTDs.Events;

public class MotdEventAnnouncer(MotdEventHandler eventHandler, ILogger<MotdEventAnnouncer> logger,
    IServiceProvider serviceProvider, DiscordSocketClient client) : IEvent
{
    private readonly DiscordSocketClient _client = client;
    private readonly MotdEventHandler _eventHandler = eventHandler;
    private readonly ILogger<MotdEventAnnouncer> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void RegisterEvents()
    {
        _eventHandler.OnGuildMotdCreated += async (a, b) => await AnnounceMotd(a, b, RestAction.Created);

        _eventHandler.OnGuildMotdUpdated += async (a, b) => await AnnounceMotd(a, b, RestAction.Updated);
    }

    private async Task AnnounceMotd(GuildMotd motd, IUser actor, RestAction action)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation($"Announcing motd {motd.GuildId} ({motd.Id}).");

        var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
            .GetGuildConfig(motd.GuildId);

        _logger.LogInformation(
            $"Sending internal webhook for motd {motd.GuildId} ({motd.Id}) to {guildConfig.StaffAnnouncements}.");

        try
        {
            var embed = await motd.CreateMotdEmbed(actor, action, _serviceProvider);

            await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffAnnouncements, embed);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while announcing motd {motd.GuildId} ({motd.Id}) to {guildConfig.StaffAnnouncements}.");
        }
    }
}
