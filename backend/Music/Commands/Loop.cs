using Bot.Attributes;
using Discord;
using Discord.Interactions;
using Lavalink4NET.Players.Queued;
using Music.Abstractions;

namespace Music.Commands;

public class LoopCommand : MusicCommand<LoopCommand>
{
    [SlashCommand("loop", "Changes the current loop mode")]
    [BotChannel]
    public async Task Loop(TrackRepeatMode loopMode)
    {
        var track = Player.CurrentTrack;

        if (track == null)
        {
            await RespondInteraction("Unable to get the track, maybe because I am not playing anything");

            return;
        }

        Player.RepeatMode = loopMode;

        await RespondInteraction(
                $"{(Player.RepeatMode != TrackRepeatMode.None ?
                    $"Looping the {Player.RepeatMode switch
                    {
                        TrackRepeatMode.Track => "track",
                        TrackRepeatMode.Queue => "queue",
                        _ => "UNKNOWN"
                    }}" :
                    "Removed the loop of")}: {Format.Bold(Format.Sanitize(track.Title))} by {Format.Bold(Format.Sanitize(track.Author))}"
        );
    }
}
