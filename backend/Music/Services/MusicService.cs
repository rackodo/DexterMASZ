using Bot.Abstractions;
using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Events;
using Lavalink4NET.Player;
using Lavalink4NET.Tracking;

namespace Music.Services;

public class MusicService : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly InactivityTrackingService _inactivityTracker;
    private readonly IAudioService _lavalink;

    public readonly Dictionary<ulong, ulong> GuildMusicChannel;
    public object ChannelLocker;

    public readonly Dictionary<ulong, DateTime> StartTimes;
    public object StartLocker;

    public MusicService(DiscordSocketClient client, IAudioService lavalink, InactivityTrackingService inactivityTracker)
    {
        _client = client;
        _lavalink = lavalink;
        _inactivityTracker = inactivityTracker;

        GuildMusicChannel = new Dictionary<ulong, ulong>();
        ChannelLocker = new object();

        StartTimes = new Dictionary<ulong, DateTime>();
        StartLocker = new object();
    }

    public void RegisterEvents()
    {
        _client.Ready += SetupLavalink;
        _client.UserVoiceStateUpdated += CheckLeft;

        _lavalink.TrackStarted += OnTrackStarted;
        _lavalink.TrackStuck += OnTrackStuck;
        _lavalink.TrackEnd += OnTrackEnd;
        _lavalink.TrackException += OnTrackException;
    }

    public async Task OnTrackStarted(object _, TrackStartedEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await GetMusicChannel(e.Player)
            .SendMessageAsync(
                $"Now playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} " +
                $"by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}"
            );
    }

    public async Task OnTrackStuck(object _, TrackStuckEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await GetMusicChannel(e.Player)
            .SendMessageAsync(
                $"Track stuck: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} " +
                $"by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}"
            );
    }

    public async Task OnTrackEnd(object _, TrackEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await GetMusicChannel(e.Player)
            .SendMessageAsync(
                $"Finished playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} by " +
                $"{Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}"
            );
    }

    public async Task OnTrackException(object _, TrackExceptionEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await GetMusicChannel(e.Player)
            .SendMessageAsync(
                $"Error playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} " +
                $"by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}",
                embed: new EmbedBuilder()
                    .WithTitle("Error message")
                    .WithDescription(e.ErrorMessage)
                    .Build()
            );
    }

    public SocketTextChannel GetMusicChannel(LavalinkPlayer player)
    {
        ulong channelId;

        lock (ChannelLocker)
            channelId = GuildMusicChannel[player.GuildId];

        return _client.GetGuild(player.GuildId).GetTextChannel(channelId);
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

    public void SetStartTimeAsCurrent(ulong guildId)
    {
        lock (StartTimes)
        {
            if (!StartTimes.ContainsKey(guildId))
                StartTimes.Add(guildId, DateTime.UtcNow);
            else
                StartTimes[guildId] = DateTime.UtcNow;
        }
    }
    
    public DateTime GetStartTime(ulong guildId)
    {
        lock (StartTimes)
        {
            return StartTimes.ContainsKey(guildId) ?
                StartTimes[guildId] :
                DateTime.UtcNow;
        }
    }

    public void SetCurrentChannelId(ulong guildId, ulong channelId)
    {
        lock (ChannelLocker)
        {
            if (!GuildMusicChannel.ContainsKey(guildId))
                GuildMusicChannel.Add(guildId, channelId);
            else
                GuildMusicChannel[guildId] = channelId;
        }
    }
}
