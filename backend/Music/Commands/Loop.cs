using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;

namespace Music.Commands;

public class LoopCommand : MusicCommand<LoopCommand>
{
    [SlashCommand("loop", "Changes the current loop mode")]
    [BotChannel]
    public async Task Loop(PlayerLoopMode loopMode)
    {
        var track = Player.CurrentTrack;

        if (track == null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Unable to get the track, maybe because I am not playing anything");

            return;
        }

        Player.LoopMode = loopMode;

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content =
                $"{(Player.LoopMode != PlayerLoopMode.None ?
                    $"Looping the {Player.LoopMode switch
                    {
                        PlayerLoopMode.Track => "track",
                        PlayerLoopMode.Queue => "queue",
                        _ => "UNKNOWN"
                    }}" :
                    "Removed the loop of")}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}"
        );
    }
}
