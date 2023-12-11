using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;
using Music.Attributes;

namespace Music.Commands;

public class ClearMusicCommand : MusicCommand<ClearMusicCommand>
{
    [SlashCommand("clear", "Clear the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Clear()
    {
        await Player.Queue.ClearAsync();

        await RespondInteraction("Cleared all tracks of the queue");
    }
}
