using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Music.Attributes;
using Music.Data;
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
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Invalid index");

            return;
        }

        List<PageBuilder> pages = new();

        for (var p = startIndexInt; p <= endIndexInt; ++p)
        {
            var track = Player.Queue[p];
            var page = new PageBuilder();
            page.WithText(
                $"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
            pages.Add(page);
        }

        Player.Queue.RemoveRange(startIndexInt, endIndexInt - startIndexInt + 1);

        await InteractiveService.SendPaginator(pages, Context);
    }
}
