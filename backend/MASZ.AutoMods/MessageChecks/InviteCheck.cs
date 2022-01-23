using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using MASZ.AutoMods.Models;

namespace MASZ.AutoMods.MessageChecks;

public static class InviteChecker
{
	private static readonly Regex InviteRegex =
		new(@"(https?:\/\/)?(www\.)?(discord(app)?\.(gg|io|me|li|com)(\/invite)?)\/(?![a-z]+\/)([^\s]+)");

	public static async Task<bool> Check(IMessage message, AutoModConfig config, DiscordSocketClient client)
	{
		if (string.IsNullOrEmpty(message.Content))
			return false;

		List<string> ignoreGuilds = new();

		if (!string.IsNullOrEmpty(config.CustomWordFilter))
			ignoreGuilds = config.CustomWordFilter.Split('\n').ToList();

		var matches = InviteRegex.Matches(message.Content);

		if (matches.Count == 0) return false;

		List<string> alreadyChecked = new();

		foreach (Match usedInvite in matches)
			try
			{
				var inviteCode = usedInvite.Groups.Values.Last().ToString().Trim();

				if (alreadyChecked.Contains(inviteCode))
					continue;

				alreadyChecked.Add(inviteCode);
				IInvite fetchedInvite = await client.GetInviteAsync(inviteCode);

				if (fetchedInvite.Guild.Id != ((ITextChannel)message.Channel).GuildId &&
				    !ignoreGuilds.Contains(fetchedInvite.Guild.Id.ToString()))
					return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return true;
			}

		return false;

	}
}