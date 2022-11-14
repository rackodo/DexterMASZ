using System.Text.RegularExpressions;
using AutoMods.Models;
using Discord;
using Discord.WebSocket;

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

            try
            {
                matches += Regex.Matches(message.Content, word, RegexOptions.IgnoreCase).Count;
            }
            catch (RegexParseException)
            {
            }

            if (matches > config.Limit)
                break;
        }

        return matches > config.Limit;
    }
}