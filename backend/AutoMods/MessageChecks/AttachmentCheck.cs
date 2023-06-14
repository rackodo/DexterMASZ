using AutoMods.Models;
using Discord;
using Discord.WebSocket;

namespace AutoMods.MessageChecks;

public static class AttachmentCheck
{
    public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _) =>
        config.Limit != null && message.Attachments != null && message.Attachments.Count > config.Limit;
}
