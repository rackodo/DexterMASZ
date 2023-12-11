using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;
using Music.Enums;
using Music.Extensions;

namespace Music.Commands;

public class PlayStreamCommand : MusicCommand<PlayStreamCommand>
{
    [SlashCommand("play-stream", "Play a stream")]
    [BotChannel]
    public async Task PlayStream(
        [Summary("stream-url", "Stream URL")] string streamUrl,
        [Summary("source", "Music source")] MusicSource source = MusicSource.Default)
    {
        if (!Uri.IsWellFormedUriString(streamUrl, UriKind.Absolute))
        {
            await RespondInteraction("You need to provide a valid URL");
            return;
        }

        var track = await Audio.Tracks.LoadTrackAsync(streamUrl, source.GetSearchMode());

        if (track == null)
        {
            await RespondInteraction($"Unable to get the stream from {streamUrl}");
            return;
        }

        await Player.PlayAsync(track);

        await RespondInteraction($"Now streaming from {streamUrl}");
    }
}
