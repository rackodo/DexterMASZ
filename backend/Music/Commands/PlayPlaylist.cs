using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Extensions;
using System.Text;

namespace Music.Commands;

public class PlaylistCommand : MusicCommand<PlaylistCommand>
{
    public InteractiveService InteractiveService { get; set; }

    [SlashCommand("play-playlist", "Add tracks from a playlist to queue")]
    [BotChannel]
    public async Task PlayPlaylist(
        [Summary("playlist-url", "Playlist URL")]
        string playlistUrl)
    {
        if (!Uri.IsWellFormedUriString(playlistUrl, UriKind.Absolute))
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "You need to provide a valid URL");

            return;
        }

        var tracks = await Lavalink.GetTracksAsync(playlistUrl);
        var lavalinkTracks = tracks.ToList();

        if (!lavalinkTracks.Any())
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the tracks");

            return;
        }

        List<LavalinkTrack> postProcessedTracks = new();

        StringBuilder text = new();
        var idx = 0;

        foreach (var track in lavalinkTracks)
        {
            var testStr = track.IsLiveStream
                ? $"Live stream skipped: {Format.Url($"{Format.Bold(track.Title)} by {Format.Bold(track.Author)}", track.Uri?.AbsoluteUri ?? "https://example.com")}"
                : $"{idx + 1} - {Format.Url($"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(track.Author)}", track.Uri?.AbsoluteUri ?? "https://example.com")}";

            text.AppendLine(testStr);

            if (track.IsLiveStream) continue;

            ++idx;
            postProcessedTracks.Add(track);
        }

        Player.Queue.AddRange(postProcessedTracks);

        var pages = MusicPages.CreatePagesFromString(text.ToString(), "Queued Playlist", Color.Gold);
        
        if (!Player.Queue.IsEmpty)
        {
            await Player.SkipAsync();
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Playing playlist!");
            await InteractiveService.SendPaginator(pages, Context);
        }
        else
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Could not play queue!");
        }
    }
}
