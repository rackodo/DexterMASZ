using Discord;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Utils;

namespace Music.Commands.Queue;

public class Remove : QueueCommand<Remove>
{
    [SlashCommand("remove", "Remove a track from the queue")]
    public async Task RemoveMusic(
        [Summary("index", "Index to remove from 0 (first track)")]
        long index)
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;
        if (!await mmu.EnsureQueuedPlayerAsync()) return;
        if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

        var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);

        if (index < 0 || index >= player!.Queue.Count)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Invalid index");

            return;
        }

        var posInt = Convert.ToInt32(index);

        var track = player.Queue[posInt];
        player.Queue.RemoveAt(posInt);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Removed track at index {index}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
