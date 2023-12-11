using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Music.Abstractions;
using Music.Attributes;

namespace Music.Commands;

public class RemoveCommand : MusicCommand<RemoveCommand>
{
    [SlashCommand("remove", "Remove a track from the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Remove(
        [Summary("index", "Index to remove from 0 (first track)")]
        long index)
    {
        if (index < 0 || index >= Player.Queue.Count)
        {
            await RespondInteraction("Invalid index");

            return;
        }

        var posInt = Convert.ToInt32(index);

        var queuedTrack = Player.Queue[posInt];

        var track = queuedTrack.Track;

        await Player.Queue.RemoveAtAsync(posInt);

        await RespondInteraction($"Removed track at index {index}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
