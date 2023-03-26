using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;

namespace Music.Extensions;

public static class ConnectClient
{
    public static async Task<VoteLavalinkPlayer> EnsureConnected(this IAudioService lavalink, IInteractionContext context)
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

        await context.Interaction.ModifyOriginalResponseAsync(x =>
            x.Content = "Done establishing the connection.");

        return newPlayer;
    }
}
