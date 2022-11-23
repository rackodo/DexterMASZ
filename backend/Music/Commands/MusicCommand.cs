using Bot.Abstractions;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;
using Lavalink4NET;
using Lavalink4NET.Events;
using Lavalink4NET.Player;
using Music.Data;

namespace Music.Commands;

[Group("music", "Music commands")]
public partial class MusicCommand : Command<MusicCommand>
{
    private VoteLavalinkPlayer _player;
    public IAudioService Lavalink { get; set; }
    public StartRepository StartRepo { get; set; }
    public InteractiveService InteractiveService { get; set; }

    public override void BeforeExecute(ICommandInfo command) =>
        _player = Lavalink.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);

    public async Task<bool> EnsureUserInVoiceAsync()
    {
        if (((SocketGuildUser)Context.Interaction.User).VoiceState != null) return true;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Join a voice channel please");

        return false;
    }

    public async Task<bool> EnsureClientInVoiceAsync()
    {
        if (_player == null) await ConnectMusic();
        return true;
    }

    public async Task<bool> EnsureQueueIsNotEmptyAsync()
    {
        if (!_player.Queue.IsEmpty) return true;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "The queue is empty now");

        return false;
    }

    public async Task OnTrackStarted(object _, TrackStartedEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Now playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}\n" +
                "You should pin this message for playing status");
    }

    public async Task OnTrackStuck(object _, TrackStuckEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Track stuck: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}");
    }

    public async Task OnTrackEnd(object _, TrackEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Finished playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}");
    }

    public async Task OnTrackException(object _, TrackExceptionEventArgs e)
    {
        var currentTrack = e.Player.CurrentTrack;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            {
                x.Content =
                    $"Error playing: {Format.Bold(Format.Sanitize(currentTrack?.Title ?? "Unknown"))} by {Format.Bold(Format.Sanitize(currentTrack?.Author ?? "Unknown"))}";
                x.Embed = new EmbedBuilder()
                    .WithTitle("Error message")
                    .WithDescription(e.ErrorMessage)
                    .Build();
            }
        );
    }
}
