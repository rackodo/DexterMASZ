using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;
using Music.Attributes;

namespace Music.Commands;

public class PlayQueueCommand : MusicCommand<PlayQueueCommand>
{
    [SlashCommand("play-queued", "Play queued tracks")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task PlayQueued() => await Player.SkipAsync();
}
