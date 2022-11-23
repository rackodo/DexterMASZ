using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("stop", "Stop this session")]
    [BotChannel]
    public async Task StopMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        await _player.StopAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Stopped this session, the queue will be cleaned");
    }
}
