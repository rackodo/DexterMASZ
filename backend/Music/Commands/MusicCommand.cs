using Bot.Abstractions;
using Discord.Interactions;
using Fergun.Interactive;
using Lavalink4NET;
using Music.Data;

namespace Music.Commands;

[Group("music", "Music commands")]
public partial class MusicCommand : Command<MusicCommand>
{
    public IAudioService Lavalink { get; set; }
    public StartRepository StartRepo { get; set; }

    [Group("queue", "Queue commands")]
    public partial class QueueCommand : Command<QueueCommand>
    {
        public IAudioService Lavalink { get; set; }
        public InteractiveService InteractiveService { get; set; }
    }
}
