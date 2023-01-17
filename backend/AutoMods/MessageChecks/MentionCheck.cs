using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class MentionCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
    {
        if (config.Limit == null)
            return false;

        return message.MentionedUserIds == null ? false : message.MentionedUserIds.Count > config.Limit;
    }
}
