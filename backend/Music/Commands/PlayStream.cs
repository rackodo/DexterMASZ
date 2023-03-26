using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public class PlayStreamCommand : MusicCommand<PlayStreamCommand>
{
    [SlashCommand("play-stream", "Play a stream")]
    [BotChannel]
    public async Task PlayStream(
        [Summary("stream-url", "Stream URL")] string streamUrl)
    {
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

        await Player.PlayAsync(track);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = $"Now streaming from {streamUrl}");
    }
}
