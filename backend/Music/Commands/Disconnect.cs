using Discord.Interactions;
using Music.Abstractions;
using Music.Utils;

namespace Music.Commands;

public class Disconnect : MusicCommand<Disconnect>
{
    [SlashCommand("disconnect", "Leave current voice channel")]
    public async Task DisconnectMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);
        Lavalink.TrackStarted -= mmu.OnTrackStarted;
        Lavalink.TrackStuck -= mmu.OnTrackStuck;
        Lavalink.TrackEnd -= mmu.OnTrackEnd;
        Lavalink.TrackException -= mmu.OnTrackException;

        await player!.DisconnectAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Left the voice channel");
    }
}
