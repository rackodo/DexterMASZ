using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("remove range", "Remove a range of tracks from the queue")]
    public async Task RemoveRangeMusic(
        [Summary("start index", "Starting index to remove from 0 (first track)")]
        long startIndex,
        [Summary("end index", "Ending index to remove from 0 (first track)")]
        long endIndex)
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        var startIndexInt = Convert.ToInt32(startIndex);
        var endIndexInt = Convert.ToInt32(endIndex);

        if (startIndexInt < 0 || endIndexInt >= _player!.Queue.Count || startIndexInt > endIndexInt)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Invalid index");

            return;
        }

        List<PageBuilder> pages = new();

        for (var p = startIndexInt; p <= endIndexInt; ++p)
        {
            var track = _player.Queue[p];
            var page = new PageBuilder();
            page.WithText(
                $"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
            pages.Add(page);
        }

        _player.Queue.RemoveRange(startIndexInt, endIndexInt - startIndexInt + 1);

        var paginator = new StaticPaginatorBuilder()
            .AddUser(Context.User)
            .WithPages(pages)
            .Build();

        await InteractiveService.SendPaginatorAsync(paginator, Context.Channel);
    }
}
