using Discord;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("vote skip", "Skip this track")]
    public async Task SkipMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        var track = _player!.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the track, maybe because I am not playing anything");

            return;
        }

        var info = await _player.VoteAsync(Context.User.Id);

        if (info.WasSkipped)
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content =
                    $"Skipped - {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
        else await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"Votes required: {info.Votes.Count}/{Math.Floor(info.Percentage * info.TotalUsers)}")
    }
}
