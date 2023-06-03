using Bot.Attributes;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;

namespace Music.Commands;

public class PauseCommand : MusicCommand<PauseCommand>
{
    [SlashCommand("pause", "Pause this session")]
    [BotChannel]
    public async Task Pause()
    {
        if (Player.State == PlayerState.Paused)
        {
            await RespondInteraction("Paused earlier");
            return;
        }

        await Player.PauseAsync();

        await RespondInteraction("Pausing");
    }
}
