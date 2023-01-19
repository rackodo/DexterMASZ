using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Lavalink4NET.Player;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("loop", "Toggles loop")]
    [BotChannel]
    public async Task LoopMusic(PlayerLoopMode loopMode)
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

        _player.LoopMode = loopMode;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"{(_player.LoopMode != PlayerLoopMode.None ? $"Looping the {_player.LoopMode switch { PlayerLoopMode.Track => "track", PlayerLoopMode.Queue => "queue", _ => "UNKNOWN" }}" : "Removed the loop of")}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}");
    }
}
