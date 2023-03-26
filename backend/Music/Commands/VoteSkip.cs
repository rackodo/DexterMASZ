using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Music.Abstractions;

namespace Music.Commands;

public class VoteSkipCommand : MusicCommand<VoteSkipCommand>
{
    [SlashCommand("vote-skip", "Skip this track")]
    [BotChannel]
    public async Task VoteSkip()
    {
        var track = Player.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the track, maybe because I am not playing anything");

            return;
        }

        var info = await Player.VoteAsync(Context.User.Id);

        if (info.WasSkipped)
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content =
                    $"Skipped - {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
        else
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content =
                    $"Votes required: {info.Votes.Count}/{Math.Floor(info.Percentage * info.TotalUsers)}");
    }
}
