using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;
using Lavalink4NET.Lyrics;
using Music.Extensions;

namespace Music.Commands;

public partial class MusicCommand
{
    public InteractiveService InteractiveService { get; set; }
    public LyricsService LyricService { get; set; }

    [SlashCommand("lyrics", "Check lyrics of a track")]
    public async Task LyricsMusic(
        [Summary("artist", "Artist name")] string artist,
        [Summary("track_name", "Track name")] string trackName)
    {
        await Context.Interaction.DeferAsync();

        try
        {
            var lyrics = await LyricService.GetLyricsAsync(artist, trackName);

            if (string.IsNullOrWhiteSpace(lyrics))
            {
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content =
                        $"No lyrics found for: {Format.Bold(Format.Sanitize(artist))} by {Format.Bold(Format.Sanitize(trackName))}");

                return;
            }

            var paginator = new StaticPaginatorBuilder()
                .AddUser(Context.User)
                .WithPages(MusicPages.CreatePagesFromString(lyrics))
                .Build();

            await InteractiveService.SendPaginatorAsync(paginator, Context.Interaction,
                responseType: InteractionResponseType.DeferredChannelMessageWithSource);
        }
        catch
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content =
                    $"Can not get lyrics for: {Format.Bold(Format.Sanitize(artist))} by {Format.Bold(Format.Sanitize(trackName))}");
        }
    }
}
