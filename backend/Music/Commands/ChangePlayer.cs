using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Player;
using Music.Enums;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("change-player", "Change current player")]
    public async Task ChangePlayerMusic(
        [Summary("connection-type", "Connection type")]
        MusicConnectionType connectionType = MusicConnectionType.Normal)
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;
        if (!await mmu.EnsureClientInVoiceAsync()) return;

        var oldPlayer = Lavalink.GetPlayer(Context.Guild.Id);

        if (oldPlayer!.State is not PlayerState.NotPlaying)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Can not change the player because there is a pending track");

            return;
        }

        switch (oldPlayer)
        {
            case QueuedLavalinkPlayer when connectionType == MusicConnectionType.Queued:
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Already connected as queued player");

                return;

            case QueuedLavalinkPlayer:
                await oldPlayer.DisconnectAsync();
                await oldPlayer.DestroyAsync();
                await Lavalink.JoinAsync(Context.Guild.Id,
                    ((SocketGuildUser)Context.User).VoiceState!.Value.VoiceChannel.Id, true);

                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Switched to normal player");
                break;

            case not null when connectionType == MusicConnectionType.Normal:
                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Already connected as normal player");

                return;
            case not null:
                await oldPlayer.DisconnectAsync();
                await oldPlayer.DestroyAsync();
                await Lavalink.JoinAsync<QueuedLavalinkPlayer>(Context.Guild.Id,
                    ((SocketGuildUser)Context.User).VoiceState!.Value.VoiceChannel.Id, true);

                await Context.Interaction.ModifyOriginalResponseAsync(x =>
                    x.Content = "Switched to queued player");
                break;
        }
    }
}
