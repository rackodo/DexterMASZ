using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;
using Lavalink4NET.Player;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    public partial class QueueCommand
    {
        [SlashCommand("removeRange", "Remove a range of tracks from the queue")]
        public async Task RemoveRangeMusic(
            [Summary("startIndex", "Starting index to remove from 0 (first track)")]
            long startIndex,
            [Summary("endIndex", "Ending index to remove from 0 (first track)")]
            long endIndex)
        {
            await Context.Interaction.DeferAsync();

            var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
            if (!await mmu.EnsureUserInVoiceAsync()) return;
            if (!await mmu.EnsureClientInVoiceAsync()) return;
            if (!await mmu.EnsureQueuedPlayerAsync()) return;
            if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

            var startIndexInt = Convert.ToInt32(startIndex);
            var endIndexInt = Convert.ToInt32(endIndex);

            var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);

            if (startIndexInt < 0 || endIndexInt >= player!.Queue.Count || startIndexInt > endIndexInt)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Invalid index");

                return;
            }

            List<PageBuilder> pages = new();

            for (var p = startIndexInt; p <= endIndexInt; ++p)
            {
                var track = player.Queue[p];
                var page = new PageBuilder();
                page.WithText(
                    $"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
                pages.Add(page);
            }

            player.Queue.RemoveRange(startIndexInt, endIndexInt - startIndexInt + 1);

            var paginator = new StaticPaginatorBuilder()
                .AddUser(Context.User)
                .WithPages(pages)
                .Build();

            await InteractiveService.SendPaginatorAsync(paginator, Context.Channel);
        }
    }
}
