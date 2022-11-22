using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Discord.WebSocket;
using GuildAudits.Extensions;
using GuildAudits.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GuildAudits.Events;

public class GuildAuditEventAnnouncer : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly GuildAuditEventHandler _eventHandler;
    private readonly ILogger<GuildAuditEventAnnouncer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GuildAuditEventAnnouncer(GuildAuditEventHandler eventHandler, ILogger<GuildAuditEventAnnouncer> logger,
        IServiceProvider serviceProvider, DiscordSocketClient client)
    {
        _eventHandler = eventHandler;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = client;
    }

    public void RegisterEvents()
    {
        _eventHandler.OnGuildAuditConfigCreated +=
            async (a, b) => await AnnounceGuildAuditLog(a, b, RestAction.Created);

        _eventHandler.OnGuildAuditConfigUpdated +=
            async (a, b) => await AnnounceGuildAuditLog(a, b, RestAction.Updated);

        _eventHandler.OnGuildAuditConfigDeleted +=
            async (a, b) => await AnnounceGuildAuditLog(a, b, RestAction.Deleted);
    }

    private async Task AnnounceGuildAuditLog(GuildAuditConfig config, IUser actor, RestAction action)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation(
            $"Announcing guild audit log {config.GuildId}/{config.GuildAuditLogEvent} ({config.Id}).");

        var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
            .GetGuildConfig(config.GuildId);

        _logger.LogInformation(
            $"Sending internal webhook for guild audit log {config.GuildId}/{config.GuildAuditLogEvent} ({config.Id}) to {guildConfig.StaffLogs}.");

        try
        {
            var embed = await config.CreateGuildAuditEmbed(actor, action, scope.ServiceProvider);

            await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffLogs, embed);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while announcing guild audit log {config.GuildId}/{config.GuildAuditLogEvent} ({config.Id}) to {guildConfig.StaffLogs}.");
        }
    }
}
