using Bot.Attributes;
using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Attributes;

namespace Music.Commands;

public class ResumeCommand : MusicCommand<ResumeCommand>
{
    [SlashCommand("resume", "Resume this session")]
    [BotChannel]
    [QueueNotEmpty]
    public async Task Resume()
    {
        if (Player.State != PlayerState.Paused)
        {
            await RespondInteraction("Resumed earlier");
            return;
        }

        await Player.ResumeAsync();

        await RespondInteraction("Resuming");
    }
}
