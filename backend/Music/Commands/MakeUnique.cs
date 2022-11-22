using Discord.Interactions;
using Lavalink4NET.Player;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    public partial class QueueCommand
    {
        [SlashCommand("makeUnique", "Remove duplicating tracks from the list")]
        public async Task MakeUniqueMusic()
        {
            await Context.Interaction.DeferAsync();

            var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
            if (!await mmu.EnsureUserInVoiceAsync()) return;
            if (!await mmu.EnsureClientInVoiceAsync()) return;
            if (!await mmu.EnsureQueuedPlayerAsync()) return;
            if (!await mmu.EnsureQueueIsNotEmptyAsync()) return;

            var player = Lavalink.GetPlayer<QueuedLavalinkPlayer>(Context.Guild.Id);

            player!.Queue.Distinct();

            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Removed duplicating tracks with same source from the queue");
        }
    }
}
