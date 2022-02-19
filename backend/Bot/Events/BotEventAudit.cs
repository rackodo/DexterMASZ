using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Bot.Identities;
using Bot.Models;
using Bot.Services;

namespace Bot.Events;

public class BotEventAudit : Event
{
	private readonly AuditLogger _auditLogger;
	private readonly DiscordRest _discordRest;
	private readonly BotEventHandler _eventHandler;

	public BotEventAudit(AuditLogger auditLogger, DiscordRest discordRest, BotEventHandler eventHandler)
	{
		_auditLogger = auditLogger;
		_discordRest = discordRest;
		_eventHandler = eventHandler;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnGuildUpdated += OnGuildUpdatedAudit;
		_eventHandler.OnGuildDeleted += OnGuildDeletedAudit;
		_eventHandler.OnGuildRegistered += OnGuildRegisteredAudit;

		_eventHandler.OnInternalCachingDone += OnInternalCachingDoneAudit;
		_eventHandler.OnIdentityRegistered += OnIdentityRegisteredAudit;
	}

	private Task OnIdentityRegisteredAudit(Identity identity)
	{
		if (identity is not DiscordOAuthIdentity dOauth)
			return Task.CompletedTask;

		var currentUser = dOauth.GetCurrentUser();
		var userDefinition = $"`{currentUser.Username}#{currentUser.Discriminator}` (`{currentUser.Id}`)";
		_auditLogger.QueueLog($"{userDefinition} **logged in** using OAuth.");

		return Task.CompletedTask;
	}

	private Task OnGuildDeletedAudit(GuildConfig guildConfig)
	{
		_auditLogger.QueueLog($"**Guild** `{guildConfig.GuildId}` deleted.");
		return Task.CompletedTask;
	}

	private Task OnGuildUpdatedAudit(GuildConfig guildConfig)
	{
		_auditLogger.QueueLog($"**Guild** `{guildConfig.GuildId}` updated.");
		return Task.CompletedTask;
	}

	private Task OnGuildRegisteredAudit(GuildConfig guildConfig, bool importExistingBans)
	{
		_auditLogger.QueueLog($"**Guild** `{guildConfig.GuildId}` registered.");
		return Task.CompletedTask;
	}

	private async Task OnInternalCachingDoneAudit(int _, DateTime nextCache)
	{
		_auditLogger.QueueLog($"Internal cache refreshed with `{_discordRest.GetCache().Keys.Count}` entries. " +
							  $"Next cache refresh {nextCache.ToDiscordTs(DiscordTimestampFormats.RelativeTime)}.");

		await _auditLogger.ExecuteWebhook();
	}
}