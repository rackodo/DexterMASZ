using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Music.Abstractions;
using Music.Attributes;
using Music.Extensions;

namespace Music.Commands;

public class RemoveRangeCommand : MusicCommand<RemoveRangeCommand>
{
    public InteractiveService InteractiveService { get; set; }

    [SlashCommand("remove-range", "Remove a range of tracks from the queue")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task RemoveRange(
        [Summary("start-index", "Starting index to remove from 0 (first track)")]
        long startIndex,
        [Summary("end-index", "Ending index to remove from 0 (first track)")]
        long endIndex)
    {
        var startIndexInt = Convert.ToInt32(startIndex);
        var endIndexInt = Convert.ToInt32(endIndex);

        if (startIndexInt < 0 || endIndexInt >= Player.Queue.Count || startIndexInt > endIndexInt)
        {
            await RespondInteraction("Invalid index");
            return;
        }

        List<PageBuilder> pages = [];

        for (var p = startIndexInt; p <= endIndexInt; ++p)
        {
            var queuedTrack = Player.Queue[p];
            var track = queuedTrack.Track;

            var page = new PageBuilder();
            page.WithText(
                $"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
            pages.Add(page);
        }

        await Player.Queue.RemoveRangeAsync(startIndexInt, endIndexInt - startIndexInt + 1);

        await RespondInteraction("Sending removed songs...");
        await InteractiveService.SendPaginator(pages, Context);
    }
}
