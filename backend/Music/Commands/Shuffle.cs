using Bot.Attributes;
using Discord.Interactions;
using Music.Attributes;
using System.Numerics;

namespace Music.Commands;

public class ShuffleCommand : MusicCommand<ShuffleCommand>
{
    [SlashCommand("shuffle", "Shuffle the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Shuffle()
    {
        Player.Queue.Shuffle();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Shuffled the queue");
    }
}
