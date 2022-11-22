using Discord;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Utils;

namespace Music.Commands;

public class Loop : MusicCommand<Loop>
{
    [SlashCommand("loop", "Toggle current track loop")]
    public async Task LoopMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));

        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;
        if (!await mmu.EnsureQueuedPlayerAsync()) return;

        var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);
        var track = player!.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the track, maybe because I am not playing anything");

            return;
        }

        player.IsLooping = !player.IsLooping;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"{(player.IsLooping ? "Looping" : "Removed the loop of")} the track: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
