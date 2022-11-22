using Bot.Abstractions;
using Discord;
using UserMaps.Models;

namespace UserMaps.Events;

public class UserMapEventHandler : INternalEventHandler
{
    internal readonly AsyncEvent<Func<UserMap, IUser, Task>> UserMapCreatedEvent = new();

    internal readonly AsyncEvent<Func<UserMap, IUser, Task>> UserMapDeletedEvent = new();

    internal readonly AsyncEvent<Func<UserMap, IUser, Task>> UserMapUpdatedEvent = new();

    public event Func<UserMap, IUser, Task> OnUserMapCreated
    {
        add => UserMapCreatedEvent.Add(value);
        remove => UserMapCreatedEvent.Remove(value);
    }

    public event Func<UserMap, IUser, Task> OnUserMapUpdated
    {
        add => UserMapUpdatedEvent.Add(value);
        remove => UserMapUpdatedEvent.Remove(value);
    }

    public event Func<UserMap, IUser, Task> OnUserMapDeleted
    {
        add => UserMapDeletedEvent.Add(value);
        remove => UserMapDeletedEvent.Remove(value);
    }
}
