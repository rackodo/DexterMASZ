using Discord;
using Discord.Interactions;
using Humanizer;
using Lavalink4NET.Artwork;
using Lavalink4NET.Player;
using Music.Extensions;

namespace Music.Commands;

public partial class MusicCommand
{
    public ArtworkService ArtworkService { get; set; }

    [SlashCommand("now playing", "View now playing track")]
    public async Task NowPlayingMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        var track = _player!.CurrentTrack;

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
                    : $"{_player.Position.RelativePosition:g}/{track.Duration:g}", true)
                .AddField("Is looping", _player.IsLooping, true)
                .AddField("Is paused", $"{_player.State == PlayerState.Paused}", true).Build());
    }
}
