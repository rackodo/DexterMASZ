using Bot.Attributes;
using Discord;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("remove", "Remove a track from the queue")]
    [BotChannel]
    public async Task RemoveMusic(
        [Summary("index", "Index to remove from 0 (first track)")]
        long index)
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        if (index < 0 || index >= _player.Queue.Count)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Invalid index");

            return;
        }

        var posInt = Convert.ToInt32(index);

        var track = _player.Queue[posInt];
        _player.Queue.RemoveAt(posInt);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Removed track at index {index}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
