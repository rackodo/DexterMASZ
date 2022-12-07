using Bot.Attributes;
using Discord.Interactions;
using Lavalink4NET.Player;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play-queued", "Play queued tracks")]
    [BotChannel]
    public async Task PlayQueueMusic()
    {
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        await Context.Interaction.DeferAsync();

        await PlayQueue();
    }

    public async Task<bool> PlayQueue()
    {
        if (!await EnsureUserInVoiceAsync()) return false;
        if (!await EnsureClientInVoiceAsync()) return false;
        if (_player.Queue.IsEmpty) return false;

        Console.WriteLine(_player.CurrentTrack);
        Console.WriteLine(_player.State);

        if (_player.CurrentTrack != null && _player.State == PlayerState.Playing)
            return false;

        Console.WriteLine("TRUE");
        var track = _player.Queue.Dequeue();

        await _player.PlayAsync(track, false);

        Lavalink.TrackStarted += OnTrackStarted;
        Lavalink.TrackStuck += OnTrackStuck;
        Lavalink.TrackEnd += OnTrackEnd;
        Lavalink.TrackException += OnTrackException;

        return true;
    }
}
