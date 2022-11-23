using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play-queued", "Play queued tracks")]
    public async Task PlayQueueMusic()
    {
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        await Context.Interaction.DeferAsync();

        await PlayQueue();
    }

    public async Task PlayQueue()
    {
        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (_player.Queue.IsEmpty) return;

        var track = _player.Queue.Dequeue();

        await _player.PlayAsync(track, false);

        Lavalink.TrackStarted += OnTrackStarted;
        Lavalink.TrackStuck += OnTrackStuck;
        Lavalink.TrackEnd += OnTrackEnd;
        Lavalink.TrackException += OnTrackException;
    }
}
