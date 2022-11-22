using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Abstractions;
using Music.Utils;

namespace Music.Commands;

public class Resume : MusicCommand<Resume>
{
    [SlashCommand("resume", "Resume this session")]
    public async Task ResumeMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);

        if (player!.State != PlayerState.Paused)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Resumed earlier");

            return;
        }

        await player.ResumeAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Resuming");
    }
}
