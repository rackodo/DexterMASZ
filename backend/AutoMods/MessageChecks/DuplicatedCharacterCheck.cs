using Discord;
using Discord.WebSocket;
using AutoMods.Models;
using System.Text.RegularExpressions;

namespace AutoMods.MessageChecks;

public static class DuplicatedCharacterCheck
{
	public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
	{
		if (config.Limit == null)
			return false;

		if (string.IsNullOrEmpty(message.Content))
			return false;

		if (config.Limit <= 0)
			return false;

		Regex regexPattern = new(@"([^0-9`])(?:\s*\1){" + config.Limit + @",}");

		return regexPattern.Match(message.Content).Success;
	}
}