using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive.Pagination;
using Music.Extensions;
using System.Text;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("queue", "Check the queue of current playing session")]
    [BotChannel]
    public async Task ViewMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        var queue = _player.Queue;

        var idx = 0;

        StringBuilder text = new();

        foreach (var track in queue)
        {
            var testStr =
                $"{idx + 1} - {Format.Url($"{Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(track.Author)}", track.Uri?.AbsoluteUri ?? "https://example.com")}";
            text.AppendLine(testStr);
            ++idx;
        }

        var pages = MusicPages.CreatePagesFromString(text.ToString(), "Song Queue", Color.Gold).ToList();

        if (_player.CurrentTrack != null)
            pages.First().AddField("Current Track", _player.CurrentTrack.Title);

        var paginator = new StaticPaginatorBuilder()
            .AddUser(Context.User)
            .WithPages(pages)
            .Build();

        await InteractiveService.SendPaginatorAsync(paginator, Context.Interaction,
            responseType: InteractionResponseType.DeferredChannelMessageWithSource);
    }
}
