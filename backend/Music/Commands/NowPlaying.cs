using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Humanizer;
using Lavalink4NET.Artwork;
using Lavalink4NET.Player;
using Music.Data;
using Music.Extensions;

namespace Music.Commands;

public class NowPlayingCommand : MusicCommand<NowPlayingCommand>
{
    public StartRepository StartRepo { get; set; }
    public ArtworkService ArtworkService { get; set; }

    [SlashCommand("now-playing", "View now playing track")]
    [BotChannel]
    public async Task NowPlaying()
    {
        var track = Player.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the playing track");

            return;
        }

        var isStream = track.IsLiveStream;
        var art = await ArtworkService.ResolveAsync(track);

        var startTime = await StartRepo.GetGuildStartTime(Context.Guild.Id);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Embed = Context.User.CreateEmbedWithUserData()
                .WithAuthor("Currently playing track", Context.Client.CurrentUser.GetAvatarUrl())
                .WithThumbnailUrl(art?.OriginalString ?? "")
                .AddField("Title", Format.Sanitize(track.Title))
                .AddField("Author", Format.Sanitize(track.Author), true)
                .AddField("Source", Format.Sanitize(track.Uri?.AbsoluteUri ?? "Unknown"), true)
                .AddField(isStream ? "Playtime" : "Position", isStream
                    ? DateTime.UtcNow.Subtract(startTime.RadioStartTime).Humanize()
                    : $"{Player.Position.RelativePosition:g}/{track.Duration:g}", true)
                .AddField("Is Looping", $"{Player.LoopMode != PlayerLoopMode.None} ({Player.LoopMode})", true)
                .AddField("Is Paused", $"{Player.State == PlayerState.Paused}", true).Build());
    }
}
