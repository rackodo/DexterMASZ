using Bot.Abstractions;
using Bot.Models;
using Discord;
using Punishments.Models;

namespace Punishments.Events;

public class PunishmentEventHandler : InternalEventHandler
{
	internal readonly AsyncEvent<Func<ModCaseTemplate, Task>> CaseTemplateCreatedEvent = new();

	internal readonly AsyncEvent<Func<ModCaseTemplate, Task>> CaseTemplateDeletedEvent = new();

	internal readonly AsyncEvent<Func<UploadedFile, ModCase, IUser, Task>> FileDeletedEvent = new();

	internal readonly AsyncEvent<Func<UploadedFile, ModCase, IUser, Task>> FileUploadedEvent = new();

	internal readonly AsyncEvent<Func<ModCaseComment, IUser, Task>> ModCaseCommentCreatedEvent = new();

	internal readonly AsyncEvent<Func<ModCaseComment, IUser, Task>> ModCaseCommentDeletedEvent = new();

	internal readonly AsyncEvent<Func<ModCaseComment, IUser, Task>> ModCaseCommentUpdatedEvent = new();

	internal readonly AsyncEvent<Func<ModCase, IUser, bool, bool, Task>> ModCaseCreatedEvent = new();

	internal readonly AsyncEvent<Func<ModCase, IUser, bool, bool, Task>> ModCaseDeletedEvent = new();

	internal readonly AsyncEvent<Func<ModCase, IUser, bool, bool, Task>> ModCaseMarkedToBeDeletedEvent = new();

	internal readonly AsyncEvent<Func<ModCase, Task>> ModCaseRestoredEvent = new();

	internal readonly AsyncEvent<Func<ModCase, IUser, bool, bool, Task>> ModCaseUpdatedEvent = new();

	public event Func<ModCaseComment, IUser, Task> OnModCaseCommentCreated
	{
		add => ModCaseCommentCreatedEvent.Add(value);
		remove => ModCaseCommentCreatedEvent.Remove(value);
	}

	public event Func<ModCaseComment, IUser, Task> OnModCaseCommentUpdated
	{
		add => ModCaseCommentUpdatedEvent.Add(value);
		remove => ModCaseCommentUpdatedEvent.Remove(value);
	}

	public event Func<ModCaseComment, IUser, Task> OnModCaseCommentDeleted
	{
		add => ModCaseCommentDeletedEvent.Add(value);
		remove => ModCaseCommentDeletedEvent.Remove(value);
	}

	public event Func<ModCase, IUser, bool, bool, Task> OnModCaseCreated
	{
		add => ModCaseCreatedEvent.Add(value);
		remove => ModCaseCreatedEvent.Remove(value);
	}

	public event Func<ModCase, IUser, bool, bool, Task> OnModCaseUpdated
	{
		add => ModCaseUpdatedEvent.Add(value);
		remove => ModCaseUpdatedEvent.Remove(value);
	}

	public event Func<ModCase, IUser, bool, bool, Task> OnModCaseDeleted
	{
		add => ModCaseDeletedEvent.Add(value);
		remove => ModCaseDeletedEvent.Remove(value);
	}

	public event Func<ModCase, IUser, bool, bool, Task> OnModCaseMarkedToBeDeleted
	{
		add => ModCaseMarkedToBeDeletedEvent.Add(value);
		remove => ModCaseMarkedToBeDeletedEvent.Remove(value);
	}

	public event Func<ModCase, Task> OnModCaseRestored
	{
		add => ModCaseRestoredEvent.Add(value);
		remove => ModCaseRestoredEvent.Remove(value);
	}

	public event Func<ModCaseTemplate, Task> OnCaseTemplateCreated
	{
		add => CaseTemplateCreatedEvent.Add(value);
		remove => CaseTemplateCreatedEvent.Remove(value);
	}

	public event Func<ModCaseTemplate, Task> OnCaseTemplateDeleted
	{
		add => CaseTemplateDeletedEvent.Add(value);
		remove => CaseTemplateDeletedEvent.Remove(value);
	}

	public event Func<UploadedFile, ModCase, IUser, Task> OnFileUploaded
	{
		add => FileUploadedEvent.Add(value);
		remove => FileUploadedEvent.Remove(value);
	}

	public event Func<UploadedFile, ModCase, IUser, Task> OnFileDeleted
	{
		add => FileDeletedEvent.Add(value);
		remove => FileDeletedEvent.Remove(value);
	}
}