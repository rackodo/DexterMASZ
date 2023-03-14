using Bot.Abstractions;
using Discord;
using Levels.Models;

namespace Levels.Events;

public class LevelsEventHandler : INternalEventHandler
{
    internal readonly AsyncEvent<Func<GuildLevelConfig, Task>> GuildLevelConfigCreatedEvent = new();

    internal readonly AsyncEvent<Func<GuildLevelConfig, Task>> GuildLevelConfigDeletedEvent = new();

    internal readonly AsyncEvent<Func<GuildLevelConfig, Task>> GuildLevelConfigUpdatedEvent = new();
    internal readonly AsyncEvent<Func<GuildUserLevel, int, IGuildUser, IChannel, Task>> UserLevelUpEvent = new();

    public event Func<GuildUserLevel, int, IGuildUser, IChannel, Task> OnUserLevelUp
    {
        add => UserLevelUpEvent.Add(value);
        remove => UserLevelUpEvent.Remove(value);
    }

    public event Func<GuildLevelConfig, Task> OnGuildLevelConfigCreated
    {
        add => GuildLevelConfigCreatedEvent.Add(value);
        remove => GuildLevelConfigCreatedEvent.Remove(value);
    }

    public event Func<GuildLevelConfig, Task> OnGuildLevelConfigUpdated
    {
        add => GuildLevelConfigUpdatedEvent.Add(value);
        remove => GuildLevelConfigUpdatedEvent.Remove(value);
    }

    public event Func<GuildLevelConfig, Task> OnGuildLevelConfigDeleted
    {
        add => GuildLevelConfigDeletedEvent.Add(value);
        remove => GuildLevelConfigDeletedEvent.Remove(value);
    }
}
