using Discord.Interactions;
using Lavalink4NET.Player;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("pause", "Pause this session")]
    public async Task MusicPlaybackPauseCommand()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        if (_player.State == PlayerState.Paused)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Paused earlier");

            return;
        }

        await _player.PauseAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Pausing");
    }
}
