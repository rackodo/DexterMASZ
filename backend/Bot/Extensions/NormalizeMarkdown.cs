namespace Bot.Extensions;

public static class NormalizeMarkdownCharacters
{
    public static string NormalizeMarkdown(this string originalText) =>
        originalText
            .Replace("*", "\\*")
            .Replace("_", "\\_")
            .Replace("~", "\\~")
            .Replace("|", "\\|")
            .Replace(">", "\\>")
            .Replace("`", "\\`");
}
