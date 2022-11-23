using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("play-stream", "Play a stream")]
    public async Task PlayStreamMusic(
        [Summary("stream-url", "Stream URL")] string streamUrl)
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

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

        await _player!.PlayAsync(track);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = $"Now streaming from {streamUrl}");

        await StartRepo.SetGuildStartTime(Context.Guild.Id, DateTime.UtcNow);
    }
}
