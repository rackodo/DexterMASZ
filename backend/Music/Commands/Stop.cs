using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;

namespace Music.Commands;

public class StopCommand : MusicCommand<StopCommand>
{
    [SlashCommand("stop", "Stop this session")]
    [BotChannel]
    public async Task Stop()
    {
        await Player.StopAsync();

        await RespondInteraction("Stopped this session, the queue has been cleaned");
    }
}
