using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class AttachmentCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
    {
        return config.Limit == null ? false : message.Attachments == null ? false : message.Attachments.Count > config.Limit;
    }
}
