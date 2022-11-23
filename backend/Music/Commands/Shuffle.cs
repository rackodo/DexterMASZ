using Discord.Interactions;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("shuffle", "Shuffle the queue")]
    public async Task ShuffleMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;
        if (!await EnsureQueueIsNotEmptyAsync()) return;

        _player.Queue.Shuffle();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Shuffled the queue");
    }
}
