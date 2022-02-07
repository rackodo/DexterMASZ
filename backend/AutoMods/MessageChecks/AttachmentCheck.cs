using Discord;
using Discord.WebSocket;
using AutoMods.Models;

namespace AutoMods.MessageChecks;

public static class AttachmentCheck
{
	public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
	{
		if (config.Limit == null)
			return false;

		if (message.Attachments == null)
			return false;

		return message.Attachments.Count > config.Limit;
	}
}