using Discord.Interactions;
using Discord.WebSocket;
using Lavalink4NET.Player;
using Music.Enums;
using Music.Utils;

namespace Music.Commands;

public partial class MusicCommand
{
    [SlashCommand("join", "Connect to your current voice channel")]
    public async Task ConnectMusic(
        [Summary("connectionType", "Connection type")]
        MusicConnectionType connectionType = MusicConnectionType.Normal)
    {
        await Context.Interaction.DeferAsync();

        var mmu = new MusicModuleUtils(Context.Interaction, Lavalink.GetPlayer(Context.Guild.Id));
        if (!await mmu.EnsureUserInVoiceAsync()) return;

        var player = Lavalink.GetPlayer(Context.Guild.Id);
        var existedPlayerType = player is QueuedLavalinkPlayer ? "queued player" : "normal player";
        var newPlayerType = connectionType == MusicConnectionType.Queued ? "queued player" : "normal player";

        if (player != null)
        {
            await Context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = $"Already connected as {existedPlayerType}");

            return;
        }

        if (connectionType == MusicConnectionType.Queued)
            await Lavalink.JoinAsync<QueuedLavalinkPlayer>(Context.Guild.Id,
                ((SocketGuildUser)Context.User).VoiceState!.Value.VoiceChannel.Id, true);
        else
            await Lavalink.JoinAsync(Context.Guild.Id,
                ((SocketGuildUser)Context.User).VoiceState!.Value.VoiceChannel.Id, true);

        await Context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = $"Done establishing the connection as {newPlayerType}");
    }
}
