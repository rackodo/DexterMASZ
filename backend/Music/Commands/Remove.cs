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
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Invalid index");

            return;
        }

        var posInt = Convert.ToInt32(index);

        var track = Player.Queue[posInt];
        Player.Queue.RemoveAt(posInt);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Removed track at index {index}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
