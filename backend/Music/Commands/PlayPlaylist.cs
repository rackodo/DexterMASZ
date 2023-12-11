using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Lavalink4NET.Players;
using Lavalink4NET.Rest.Entities.Tracks;
using Lavalink4NET.Tracks;
using Music.Abstractions;
using Music.Enums;
using Music.Extensions;
using System.Text;

namespace Music.Commands;

public class PlaylistCommand : MusicCommand<PlaylistCommand>
{
    public InteractiveService InteractiveService { get; set; }

    [SlashCommand("play-playlist", "Add tracks from a playlist to queue")]
    [BotChannel]
    public async Task PlayPlaylist(
        [Summary("playlist-url", "Playlist URL")] string playlistUrl,
        [Summary("source", "Music source")] MusicSource source = MusicSource.Default)
    {
        if (!Uri.IsWellFormedUriString(playlistUrl, UriKind.Absolute))
        {
            await RespondInteraction("You need to provide a valid URL");
            return;
        }

        var tracks = await Audio.Tracks.LoadTracksAsync(playlistUrl, source.GetSearchMode());
        var lavalinkTracks = tracks.Tracks.ToList();

        if (lavalinkTracks.Count == 0)
        {
            await RespondInteraction("Unable to get the tracks");
            return;
        }

        List<LavalinkTrack> postProcessedTracks = [];

        StringBuilder text = new();
        var idx = 0;

        foreach (var track in lavalinkTracks)
        {
            var url = Format.Url($"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(track.Author)}",
                track.Uri?.AbsoluteUri ?? "https://unknown.com");

            var testStr = track.IsLiveStream
                ? $"Live stream skipped: {url}"
                : $"{idx + 1} - {url}";

            text.AppendLine(testStr);

            if (track.IsLiveStream) continue;

            ++idx;
            postProcessedTracks.Add(track);
        }

        foreach (var track in postProcessedTracks)
            await Player.PlayAsync(track);

        var pages = MusicPages.CreatePagesFromString(text.ToString(), "Queued Playlist", Color.Gold);

        if (!Player.Queue.IsEmpty)
        {
            if (Player.State != PlayerState.Playing)
                if (Player.CurrentTrack != null)
                    await Player.ResumeAsync();
                else
                    await Player.SkipAsync();

            await RespondInteraction("Playing playlist!");
            await InteractiveService.SendPaginator(pages, Context);
        }
        else
        {
            await RespondInteraction("Could not play queue!");
        }
    }
}
