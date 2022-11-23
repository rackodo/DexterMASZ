using Discord;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("loop", "Toggle current track loop")]
    public async Task LoopMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        var track = _player.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the track, maybe because I am not playing anything");

            return;
        }

        _player.IsLooping = !_player.IsLooping;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"{(_player.IsLooping ? "Looping" : "Removed the loop of")} the track: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
