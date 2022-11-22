using Discord;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("skip", "Skip this track")]
    public async Task SkipMusic()
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

        await player.SkipAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Skipped - {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
