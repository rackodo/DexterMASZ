using Discord.Interactions;
using Lavalink4NET.Player;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("resume", "Resume this session")]
    public async Task ResumeMusic()
    {
        await Context.Interaction.DeferAsync();

        if (!await EnsureUserInVoiceAsync()) return;
        if (!await EnsureClientInVoiceAsync()) return;

        if (_player!.State != PlayerState.Paused)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Resumed earlier");

            return;
        }

        await _player.ResumeAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Resuming");
    }
}
