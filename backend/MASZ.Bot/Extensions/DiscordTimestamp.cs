using MASZ.Bot.Enums;

namespace MASZ.Bot.Extensions;

public static class DiscordTimestamp
{
	public static string ToDiscordTs(this DateTime dateTime,
		DiscordTimestampFormats format = DiscordTimestampFormats.ShortDateTime)
	{
		return $"<t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:{(char)format}>";
	}
}