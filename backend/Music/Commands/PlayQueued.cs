using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play-queue", "Play queued tracks")]
    public async Task PlayQueueMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));

        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;
        if (!await mmu.EnsureQueuedPlayerAsync()) return;
        if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

        var queuedPlayer = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);
        var track = queuedPlayer!.Queue.Dequeue();

        await queuedPlayer.PlayAsync(track, false);

        Lavalink.TrackStarted += mmu.OnTrackStarted;
        Lavalink.TrackStuck += mmu.OnTrackStuck;
        Lavalink.TrackEnd += mmu.OnTrackEnd;
        Lavalink.TrackException += mmu.OnTrackException;
    }
}
