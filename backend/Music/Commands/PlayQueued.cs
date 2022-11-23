using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play queue", "Play queued tracks")]
    public async Task PlayQueueMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        var track = _player!.Queue.Dequeue();

        await _player.PlayAsync(track, false);

        Lavalink.TrackStarted += OnTrackStarted;
        Lavalink.TrackStuck += OnTrackStuck;
        Lavalink.TrackEnd += OnTrackEnd;
        Lavalink.TrackException += OnTrackException;
    }
}
