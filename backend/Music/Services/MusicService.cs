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

    public void RegisterEvents() => _client.Ready += SetupLavalink;

    private async Task SetupLavalink()
    {
        await _lavalink.InitializeAsync();
        if (!_inactivityTracker.IsTracking) _inactivityTracker.BeginTracking();
    }
}
