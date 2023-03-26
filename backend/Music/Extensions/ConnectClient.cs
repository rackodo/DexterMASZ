using Discord;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.Player;
using Music.Services;

namespace Music.Extensions;

public static class ConnectClient
{
    public static async Task<VoteLavalinkPlayer> EnsureConnected(this IAudioService lavalink,
        IInteractionContext context, MusicService music)
    {
        var player = lavalink.GetPlayer(context.Guild.Id);

        if (player != null)
            return player as VoteLavalinkPlayer;

        var newPlayer = await lavalink.JoinAsync<VoteLavalinkPlayer>(context.Guild.Id,
            ((SocketGuildUser)context.User).VoiceState!.Value.VoiceChannel.Id, true);

        music.SetStartTimeAsCurrent(context.Guild.Id);
        
        return newPlayer;
    }
}
