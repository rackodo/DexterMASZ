using Bot.Attributes;
using Discord.Interactions;
using Music.Attributes;

namespace Music.Commands;

public class ClearMusicCommand : MusicCommand<ClearMusicCommand>
{
    [SlashCommand("clear", "Clear the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Clear()
    {
        Player.Queue.Clear();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Cleared all tracks of the queue");
    }
}
