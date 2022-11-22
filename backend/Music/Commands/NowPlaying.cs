using Discord;
using Discord.Interactions;
using Humanizer;
using Lavalink4NET.Artwork;
using Lavalink4NET.Player;
using Music.Extensions;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    public ArtworkService ArtworkService { get; set; }

    [SlashCommand("nowPlaying", "View now playing track")]
    public async Task NowPlayingMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);
        var track = player!.CurrentTrack;

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
                    : $"{player.Position.RelativePosition:g}/{track.Duration:g}", true)
                .AddField("Is looping", player is QueuedLavalinkPlayer lavalinkPlayer
                    ? $"{lavalinkPlayer.IsLooping}"
                    : "This is not a queued player", true)
                .AddField("Is paused", $"{player.State == PlayerState.Paused}", true).Build());
    }
}
