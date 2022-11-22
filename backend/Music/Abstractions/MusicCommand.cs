using Bot.Abstractions;
using Discord.Interactions;
using Lavalink4NET;

namespace Music.Abstractions;

[Group("music", "Music commands")]
public abstract class MusicCommand<T> : Command<T>
{
    public IAudioService Lavalink { get; set; }
}
