using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Music.Data;

namespace Music.Extensions;

public static class ConnectClient
{
    public static async Task<VoteLavalinkPlayer> EnsureConnected(this IAudioService lavalink, IInteractionContext context, StartRepository startRepo)
    {
        var player = lavalink.GetPlayer(context.Guild.Id);

        if (player != null)
        {
            await context.Interaction.ModifyOriginalResponseAsync(x =>
                x.Content = "Already connected.");

            return player as VoteLavalinkPlayer;
        }

        var newPlayer = await lavalink.JoinAsync<VoteLavalinkPlayer>(context.Guild.Id,
            ((SocketGuildUser)context.User).VoiceState!.Value.VoiceChannel.Id, true);

        await startRepo.SetGuildStartTime(context.Guild.Id, DateTime.UtcNow);

        await context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Done establishing the connection.");

        return newPlayer;
    }
}
