using Bot.Abstractions;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Tracking;

namespace Music.Services;

public class MusicService : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly InactivityTrackingService _inactivityTracker;
    private readonly IAudioService _lavalink;

    public MusicService(DiscordSocketClient client, IAudioService lavalink, InactivityTrackingService inactivityTracker)
    {
        _client = client;
        _lavalink = lavalink;
        _inactivityTracker = inactivityTracker;
    }

    public void RegisterEvents()
    {
        _client.Ready += SetupLavalink;
        _client.UserVoiceStateUpdated += CheckLeft;
    }

    private async Task CheckLeft(SocketUser user, SocketVoiceState originalState, SocketVoiceState newState)
    {
        if (newState.VoiceChannel == null)
            if (originalState.VoiceChannel != null)
                if (originalState.VoiceChannel.ConnectedUsers.Count == 1)
                    if (originalState.VoiceChannel.ConnectedUsers.First().Id == _client.CurrentUser.Id)
                    {
                        var player = _lavalink.GetPlayer(originalState.VoiceChannel.Guild.Id);
                        if (player != null)
                            await player.DisconnectAsync();
                    }
    }

    private async Task SetupLavalink()
    {
        await _lavalink.InitializeAsync();
        if (!_inactivityTracker.IsTracking) _inactivityTracker.BeginTracking();
    }
}
