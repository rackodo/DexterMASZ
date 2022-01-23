using Discord;
using Discord.WebSocket;
using MASZ.AutoMods.Models;

namespace MASZ.AutoMods.MessageChecks;

public static class SpamCheck
{
	private static readonly Dictionary<ulong, Dictionary<ulong, List<long>>> MsgBoard = new();

	public static bool Check(IMessage message, AutoModConfig config, DiscordSocketClient _)
	{
		if (config.Limit == null)
			return false;

		if (config.TimeLimitMinutes == null)
			return false;

		if (message.Embeds == null)
			return false;

		var guildId = ((ITextChannel)message.Channel).Guild.Id;

		// Sets the guild config in msg_board if it doesn't exist.
		if (!MsgBoard.ContainsKey(guildId))
			MsgBoard[guildId] = new Dictionary<ulong, List<long>>();

		var timestamp = message.CreatedAt.ToUnixTimeSeconds();

		// Filters out messages older than TimeLimitMinutes.
		// Delta is the time minus the TimeLimitMinutes => the time messages older than should be deleted.
		var delta = timestamp - (long)config.TimeLimitMinutes;

		foreach (var userId in MsgBoard[guildId].Keys.ToList())
		{
			var newTimestamps = MsgBoard[guildId][userId].FindAll(x => x > delta);

			if (newTimestamps.Count > 0)
				MsgBoard[guildId][userId] = newTimestamps;
			else
				MsgBoard[guildId].Remove(userId);
		}

		// Add the message to the "msg_board".
		if (!MsgBoard[guildId].ContainsKey(message.Author.Id))
			MsgBoard[guildId][message.Author.Id] = new List<long> { timestamp };
		else
			MsgBoard[guildId][message.Author.Id].Add(timestamp);

		// Counts the number of messages and check them for being too high
		return MsgBoard[guildId][message.Author.Id].Count > config.Limit;
	}
}