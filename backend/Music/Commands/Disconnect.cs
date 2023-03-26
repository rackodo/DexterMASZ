using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public class DisconnectCommand : MusicCommand<DisconnectCommand>
{
    [SlashCommand("disconnect", "Leaves the current voice channel")]
    [BotChannel]
    public async Task Disconnect()
    {
        await Player.DisconnectAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Left the voice channel");
    }
}
