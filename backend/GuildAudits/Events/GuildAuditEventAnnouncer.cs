using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using Discord;
using GuildAudits.Extensions;
using GuildAudits.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GuildAudits.Events;

public class GuildAuditEventAnnouncer : Event
{
	private readonly GuildAuditEventHandler _eventHandler;
	private readonly ILogger<GuildAuditEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public GuildAuditEventAnnouncer(GuildAuditEventHandler eventHandler, ILogger<GuildAuditEventAnnouncer> logger,
		IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnGuildAuditConfigCreated +=
			async (a, b) => await AnnounceGuildAudit(a, b, RestAction.Created);

		_eventHandler.OnGuildAuditConfigUpdated +=
			async (a, b) => await AnnounceGuildAudit(a, b, RestAction.Updated);

		_eventHandler.OnGuildAuditConfigDeleted +=
			async (a, b) => await AnnounceGuildAudit(a, b, RestAction.Deleted);
	}

	private async Task AnnounceGuildAudit(GuildAuditConfig config, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation(
			$"Announcing guild audit log {config.GuildId}/{config.GuildAuditEvent} ({config.Id}).");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(config.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.ModNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for guild audit log {config.GuildId}/{config.GuildAuditEvent} ({config.Id}) to {guildConfig.ModNotificationWebhook}.");

			try
			{
				var embed = await config.CreateGuildAuditEmbed(actor, action, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing guild audit log {config.GuildId}/{config.GuildAuditEvent} ({config.Id}) to {guildConfig.ModNotificationWebhook}.");
			}
		}
	}
}