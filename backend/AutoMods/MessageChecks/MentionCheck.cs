using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class MentionCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _) => config.Limit == null ? false : message.MentionedUserIds == null ? false : message.MentionedUserIds.Count > config.Limit;
}
