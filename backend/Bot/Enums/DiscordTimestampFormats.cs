// Timestamp formats: https://discord.com/developers/docs/reference#message-formatting-formats

namespace Bot.Enums;

public enum DiscordTimestampFormats
{
	ShortTime = 't',
	LongTime = 'T',
	ShortDate = 'd',
	LongDate = 'D',
	ShortDateTime = 'f',
	LongDateTime = 'F',
	RelativeTime = 'R',
}