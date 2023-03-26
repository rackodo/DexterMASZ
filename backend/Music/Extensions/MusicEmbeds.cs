using Discord;
using Discord.WebSocket;

namespace Music.Extensions;

public static class MusicEmbeds
{
    public static EmbedBuilder CreateEmbedWithUserData(this SocketUser user) =>
        new EmbedBuilder()
            .WithTimestamp(DateTime.Now)
            .WithColor(Color.DarkRed);
}
