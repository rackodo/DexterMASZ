using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("clear", "Clear the queue")]
    [BotChannel]
    public async Task ClearMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        _player.Queue.Clear();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Cleared all tracks of the queue");
    }
}
