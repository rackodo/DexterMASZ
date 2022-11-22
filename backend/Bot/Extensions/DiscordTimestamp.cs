using Bot.Enums;

namespace Bot.Extensions;

public static class DiscordTimestamp
{
    public static string ToDiscordTs(this DateTime dateTime,
        DiscordTimestampFormats format = DiscordTimestampFormats.ShortDateTime) =>
        $"<t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:{(char)format}>";
}
