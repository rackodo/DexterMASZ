using Discord;
using Bot.Abstractions;
using Bot.Models;
using Bot.Services;
using Punishments.Models;

namespace Punishments.Events;

public class PunishmentEventAudit : Event
{
	private readonly AuditLogger _auditLogger;
	private readonly PunishmentEventHandler _punishmentEventHandler;

	public PunishmentEventAudit(AuditLogger auditLogger, PunishmentEventHandler punishmentEventHandler)
	{
		_auditLogger = auditLogger;
		_punishmentEventHandler = punishmentEventHandler;
	}

	public void RegisterEvents()
	{
		_punishmentEventHandler.OnFileUploaded += OnFileUploaded;

		_punishmentEventHandler.OnModCaseCommentDeleted += OnModCaseCommentDeleted;
		_punishmentEventHandler.OnModCaseCommentUpdated += OnModCaseCommentUpdated;
		_punishmentEventHandler.OnModCaseCommentCreated += OnModCaseCommentCreated;

		_punishmentEventHandler.OnModCaseDeleted += OnModCaseDeleted;
		_punishmentEventHandler.OnModCaseUpdated += OnModCaseUpdated;
		_punishmentEventHandler.OnModCaseCreated += OnModCaseCreated;
	}

	private Task OnFileUploaded(UploadedFile fileInfo, ModCase modCase, IUser actor)
	{
		_auditLogger.QueueLog(
			$"**File** `{fileInfo.Name}` uploaded to case {modCase.GuildId}/{modCase.CaseId} by <@{actor.Id}>.");
		return Task.CompletedTask;
	}

	private Task OnModCaseCommentDeleted(ModCaseComment modCaseComment, IUser actor)
	{
		_auditLogger.QueueLog(
			$"**Comment** `{modCaseComment.ModCase.GuildId}/{modCaseComment.ModCase.CaseId}/{modCaseComment.Id}` by <@{modCaseComment.UserId}> deleted.");
		return Task.CompletedTask;
	}

	private Task OnModCaseCommentUpdated(ModCaseComment modCaseComment, IUser actor)
	{
		_auditLogger.QueueLog(
			$"**Comment** `{modCaseComment.ModCase.GuildId}/{modCaseComment.ModCase.CaseId}/{modCaseComment.Id}` by <@{modCaseComment.UserId}> updated.");
		return Task.CompletedTask;
	}

	private Task OnModCaseCommentCreated(ModCaseComment modCaseComment, IUser actor)
	{
		_auditLogger.QueueLog(
			$"**Comment** `{modCaseComment.ModCase.GuildId}/{modCaseComment.ModCase.CaseId}/{modCaseComment.Id}` by <@{modCaseComment.UserId}> created.");
		return Task.CompletedTask;
	}

	private Task OnModCaseDeleted(ModCase modCase, IUser actor, bool announcePublic, bool announceDm)
	{
		_auditLogger.QueueLog($"**Mod case** `{modCase.GuildId}/{modCase.CaseId}` for <@{modCase.UserId}> deleted.");
		return Task.CompletedTask;
	}

	private Task OnModCaseUpdated(ModCase modCase, IUser actor, bool announcePublic, bool announceDm)
	{
		_auditLogger.QueueLog(
			$"**Mod case** `{modCase.GuildId}/{modCase.CaseId}` for <@{modCase.UserId}> by <@{modCase.LastEditedByModId}> updated.");
		return Task.CompletedTask;
	}

	private Task OnModCaseCreated(ModCase modCase, IUser actor, bool announcePublic, bool announceDm)
	{
		_auditLogger.QueueLog(
			$"**Mod case** `{modCase.GuildId}/{modCase.CaseId}` for <@{modCase.UserId}> by <@{modCase.ModId}> created.");
		return Task.CompletedTask;
	}
}