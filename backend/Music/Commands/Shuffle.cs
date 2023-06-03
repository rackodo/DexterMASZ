using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;
using Music.Attributes;

namespace Music.Commands;

public class ShuffleCommand : MusicCommand<ShuffleCommand>
{
    [SlashCommand("shuffle", "Shuffle the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Shuffle()
    {
        Player.Queue.Shuffle();

        await RespondInteraction("Shuffled the queue");
    }
}
