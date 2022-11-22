using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Utils;

namespace Music.Commands;

public class Pause : MusicCommand<Pause>
{
    [SlashCommand("pause", "Pause this session")]
    public async Task MusicPlaybackPauseCommand()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);

        if (player!.State == PlayerState.Paused)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Paused earlier");

            return;
        }

        await player.PauseAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Pausing");
    }
}
