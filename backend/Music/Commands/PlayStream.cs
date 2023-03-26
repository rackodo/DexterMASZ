using Bot.Attributes;
using Discord.Interactions;
using Fergun.Interactive;
using Music.Data;

namespace Music.Commands;

public class PlayStreamCommand : MusicCommand<PlayStreamCommand>
{
    public StartRepository StartRepo { get; set; }

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

        await StartRepo.SetGuildStartTime(Context.Guild.Id, DateTime.UtcNow);
    }
}
