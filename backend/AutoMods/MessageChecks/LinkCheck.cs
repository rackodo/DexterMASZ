using AutoMods.Models;
using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;

namespace AutoMods.MessageChecks;

public static class LinkCheck
{
	private static readonly Regex DomainRegex =
		new(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)");

	public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
	{
		if (config.Limit == null)
			return false;

		if (string.IsNullOrEmpty(message.Content))
			return false;

		var foundLinks = DomainRegex.Matches(message.Content);
		var count = foundLinks.Count;

		if (string.IsNullOrEmpty(config.CustomWordFilter)) return count > config.Limit;

		foreach (Match link in foundLinks)
			if (config.CustomWordFilter.Split('\n').Any(filtered => Regex.Match(link.Value, filtered).Success))
				count--;

		return count > config.Limit;
	}
}