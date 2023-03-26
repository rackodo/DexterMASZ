using Bot.Attributes;
using Discord.Interactions;
using Lavalink4NET.Player;

namespace Music.Commands;

public class PauseCommand : MusicCommand<PauseCommand>
{
    [SlashCommand("pause", "Pause this session")]
    [BotChannel]
    public async Task Pause()
    {
        if (Player.State == PlayerState.Paused)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Paused earlier");

            return;
        }

        await Player.PauseAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Pausing");
    }
}
