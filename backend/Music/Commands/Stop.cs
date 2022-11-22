using Discord.Interactions;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("stop", "Stop this session")]
    public async Task StopMusic()
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);
        await player!.StopAsync();

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Stopped this session, the queue will be cleaned");
    }
}
