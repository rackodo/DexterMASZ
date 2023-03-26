using Bot.Attributes;
using Discord.Interactions;
using Music.Abstractions;

namespace Music.Commands;

public class LeaveCommand : MusicCommand<LeaveCommand>
{
    [SlashCommand("leave", "Leaves the voice channel and stops the session")]
    [BotChannel]
    public async Task Leave()
    {
        await Player.StopAsync(true);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Left this session, the queue has been cleaned");
    }
}
