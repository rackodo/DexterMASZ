using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using Lavalink4NET.Lyrics;
using Music.Data;
using Music.Extensions;

namespace Music.Commands;

public class LyricsCommand : MusicCommand<LyricsCommand>
{
    public InteractiveService InteractiveService { get; set; }
    public LyricsService LyricService { get; set; }

    [SlashCommand("lyrics", "Check lyrics of a track")]
    [BotChannel]
    public async Task Lyrics(
        [Summary("artist", "Artist name")] string artist,
        [Summary("track-name", "Track name")] string trackName)
    {
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

            await InteractiveService.SendPaginator(
                MusicPages.CreatePagesFromString(lyrics, $"Lyrics for {trackName}", Color.Magenta), Context);
        }
        catch
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content =
                    $"Can not get lyrics for: {Format.Bold(Format.Sanitize(artist))} by {Format.Bold(Format.Sanitize(trackName))}");
        }
    }
}
