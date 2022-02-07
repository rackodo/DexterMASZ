using Bot.Abstractions;
using Bot.Services;
using Discord;
using MOTDs.Models;

namespace MOTDs.Events;

public class MotdEventAudit : Event
{
	private readonly AuditLogger _auditLogger;
	private readonly MotdEventHandler _eventHandler;

	public MotdEventAudit(AuditLogger auditLogger, MotdEventHandler eventHandler)
	{
		_auditLogger = auditLogger;
		_eventHandler = eventHandler;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnGuildMotdCreated += OnGuildMotdCreated;
		_eventHandler.OnGuildMotdUpdated += OnGuildMotdUpdated;
	}

	private Task OnGuildMotdCreated(GuildMotd motd, IUser actor)
	{
		_auditLogger.QueueLog($"**Motd** for guild `{motd.GuildId}` created by {actor.Mention}.");
		return Task.CompletedTask;
	}

	private Task OnGuildMotdUpdated(GuildMotd motd, IUser actor)
	{
		_auditLogger.QueueLog($"**Motd** for guild `{motd.GuildId}` updated by {actor.Mention}.");
		return Task.CompletedTask;
	}
}