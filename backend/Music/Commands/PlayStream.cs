using Discord.Interactions;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("playStream", "Play a stream")]
    public async Task PlayStreamMusic(
        [Summary("streamUrl", "Stream URL")] string streamUrl)
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        var player = Lavalink.GetPlayer(Context.Guild.Id);

        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;
        if (!await mmu.EnsureNormalPlayerAsync()) return;

        if (!Uri.IsWellFormedUriString(streamUrl, UriKind.Absolute))
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "I need a valid stream URL to function");

            return;
        }

        var track = await Lavalink.GetTrackAsync(streamUrl);

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = $"Unable to get the stream from {streamUrl}");

            return;
        }

        await player!.PlayAsync(track);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = $"Now streaming from {streamUrl}");

        await StartRepo.SetGuildStartTime(Context.Guild.Id, DateTime.UtcNow);
    }
}
