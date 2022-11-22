using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Extensions;
using Music.Utils;
using System.Text;

namespace Music.Commands.Queue;

public class View : QueueCommand<View>
{
    public InteractiveService InteractiveService { get; set; }

    [SlashCommand("view", "Check the queue of current playing session")]
    public async Task ViewMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;
        if (!await mmu.EnsureQueuedPlayerAsync()) return;
        if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

        var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);
        var queue = player!.Queue;

        var idx = 0;

        StringBuilder text = new();

        foreach (var track in queue)
        {
            var testStr =
                $"{idx + 1} - {Format.Url($"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(track.Author)}", track.Uri?.AbsolutePath ?? "https://example.com")}";
            text.AppendLine(testStr);
            ++idx;
        }

        var pages = MusicPages.CreatePagesFromString($"{text}");

        var paginator = new StaticPaginatorBuilder()
            .AddUser(Context.User)
            .WithPages(pages)
            .Build();

        await InteractiveService.SendPaginatorAsync(paginator, Context.Interaction,
            responseType: InteractionResponseType.DeferredChannelMessageWithSource);
    }
}
