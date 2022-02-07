using AutoMods.Models;
using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;

namespace AutoMods.MessageChecks;

public static class CustomWordCheck
{
	public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
	{
		if (config.Limit == null)
			return false;

		if (string.IsNullOrEmpty(config.CustomWordFilter))
			return false;

		if (string.IsNullOrEmpty(message.Content))
			return false;

		var matches = 0;

		foreach (var word in config.CustomWordFilter.Split('\n'))
		{
			if (string.IsNullOrWhiteSpace(word))
				continue;

			matches += Regex.Matches(message.Content, word, RegexOptions.IgnoreCase).Count;

			if (matches > config.Limit)
				break;
		}

		return matches > config.Limit;
	}
}