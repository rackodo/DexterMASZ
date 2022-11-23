using Bot.Attributes;
using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("remove-duplicates", "Remove duplicating tracks from the list")]
    [BotChannel]
    public async Task MakeUniqueMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        _player.Queue.Distinct();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Removed duplicating tracks with same source from the queue");
    }
}
