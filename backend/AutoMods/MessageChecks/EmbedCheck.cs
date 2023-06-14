using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class EmbedCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
    {
        return config.Limit == null ? false : message.Embeds == null ? false : message.Embeds.Count > config.Limit;
    }
}
