using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    public partial class QueueCommand
    {
        [SlashCommand("clear", "Clear the queue")]
        public async Task ClearMusic()
        {
            await Context.Interaction.DeferAsync();

            var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
            if (!await mmu.EnsureUserInVoiceAsync()) return;
            if (!await mmu.EnsureClientInVoiceAsync()) return;
            if (!await mmu.EnsureQueuedPlayerAsync()) return;
            if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

            var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);

            player!.Queue.Clear();

            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Cleared all tracks of the queue");
        }
    }
}
