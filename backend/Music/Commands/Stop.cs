using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public class StopCommand : MusicCommand<StopCommand>
{
    [SlashCommand("stop", "Stop this session")]
    [BotChannel]
    public async Task Stop()
    {
        await Player.StopAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Stopped this session, the queue will be cleaned");
    }
}
