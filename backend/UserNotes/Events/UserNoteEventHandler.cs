using Bot.Abstractions;
using Discord;
using UserNotes.Models;

namespace UserNotes.Events;

public class UserNoteEventHandler : InternalEventHandler
{
    internal readonly AsyncEvent<Func<UserNote, IUser, Task>> UserNoteCreatedEvent = new();

    internal readonly AsyncEvent<Func<UserNote, IUser, Task>> UserNoteDeletedEvent = new();

    internal readonly AsyncEvent<Func<UserNote, IUser, Task>> UserNoteUpdatedEvent = new();

    public event Func<UserNote, IUser, Task> OnUserNoteDeleted
    {
        add => UserNoteDeletedEvent.Add(value);
        remove => UserNoteDeletedEvent.Remove(value);
    }

    public event Func<UserNote, IUser, Task> OnUserNoteUpdated
    {
        add => UserNoteUpdatedEvent.Add(value);
        remove => UserNoteUpdatedEvent.Remove(value);
    }

    public event Func<UserNote, IUser, Task> OnUserNoteCreated
    {
        add => UserNoteCreatedEvent.Add(value);
        remove => UserNoteCreatedEvent.Remove(value);
    }
}