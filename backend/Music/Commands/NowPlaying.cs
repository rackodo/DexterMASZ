using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Humanizer;
using Lavalink4NET.Artwork;
using Music.Abstractions;
using Music.Extensions;

namespace Music.Commands;

public class NowPlayingCommand : MusicCommand<NowPlayingCommand>
{
    public ArtworkService ArtworkService { get; set; }

    [SlashCommand("now-playing", "View now playing track")]
    [BotChannel]
    public async Task NowPlaying()
    {
        var track = Player.CurrentTrack;

        if (track == null)
        {
            await RespondInteraction("Unable to get the playing track");

            return;
        }

        var isStream = track.IsLiveStream;
        var art = await ArtworkService.ResolveAsync(track);

        var startTime = Music.GetStartTime(Context.Guild.Id);

        await RespondInteraction(
            string.Empty,
            Context.User.CreateEmbedWithUserData()
                .WithAuthor("Playing Track", Context.Client.CurrentUser.GetAvatarUrl())
                .WithThumbnailUrl(art?.OriginalString ?? "")
                .AddField("Title", Format.Sanitize(track.Title))
                .AddField("Author", Format.Sanitize(track.Author))
                .AddField("Source", Format.Sanitize(track.Uri?.AbsoluteUri ?? "Unknown"))
                .AddField(isStream ? "Playtime" : "Position", isStream
                    ? DateTime.UtcNow.Subtract(startTime).Humanize()
                    : $"{Player.Position.RelativePosition:g}".Split('.').First() +
                      "/" +
                      $"{track.Duration:g}".Split('.').First()
                )
                .AddField("Loop Mode", Player.LoopMode)
                .AddField("Player State", Player.State)
        );
    }
}
