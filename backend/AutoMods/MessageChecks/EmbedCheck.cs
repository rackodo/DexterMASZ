using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class EmbedCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
    {
        if (config.Limit == null)
            return false;

        if (message.Embeds == null)
            return false;

        return message.Embeds.Count > config.Limit;
    }
}