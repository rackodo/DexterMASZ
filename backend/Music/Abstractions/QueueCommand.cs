using Discord.Interactions;

namespace Music.Abstractions;

[Group("queue", "Queue commands")]
public class QueueCommand<T> : MusicCommand<T>
{
}
