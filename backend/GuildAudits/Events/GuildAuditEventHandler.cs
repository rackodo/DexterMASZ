using Bot.Abstractions;
using Discord;
using GuildAudits.Models;

namespace GuildAudits.Events;

public class GuildAuditEventHandler : IInternalEventHandler
{
    internal readonly AsyncEvent<Func<GuildAuditConfig, IUser, Task>> GuildAuditConfigCreatedEvent = new();

    internal readonly AsyncEvent<Func<GuildAuditConfig, IUser, Task>> GuildAuditConfigDeletedEvent = new();

    internal readonly AsyncEvent<Func<GuildAuditConfig, IUser, Task>> GuildAuditUpdatedEvent = new();

    public event Func<GuildAuditConfig, IUser, Task> OnGuildAuditConfigCreated
    {
        add => GuildAuditConfigCreatedEvent.Add(value);
        remove => GuildAuditConfigCreatedEvent.Remove(value);
    }

    public event Func<GuildAuditConfig, IUser, Task> OnGuildAuditConfigUpdated
    {
        add => GuildAuditUpdatedEvent.Add(value);
        remove => GuildAuditUpdatedEvent.Remove(value);
    }

    public event Func<GuildAuditConfig, IUser, Task> OnGuildAuditConfigDeleted
    {
        add => GuildAuditConfigDeletedEvent.Add(value);
        remove => GuildAuditConfigDeletedEvent.Remove(value);
    }
}
