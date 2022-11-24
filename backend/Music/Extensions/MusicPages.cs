using Discord;
using Fergun.Interactive;
using System.Text;

namespace Music.Extensions;

public static class MusicPages
{
    public static IEnumerable<PageBuilder> CreatePagesFromString(string content, string title, Color color,
        int fixedPageSplit = 15, int threshold = 2000)
    {
        var lines = content
            .Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var idx = 0;
        List<PageBuilder> pages = new();
        StringBuilder text = new();

        foreach (var line in lines)
        {
            if (idx != 0 && idx % fixedPageSplit == 0 || text.Length + line.Length > threshold)
            {
                pages.Add(text.GetCurrentPage(title, color));
                text.Clear();
            }

            text.AppendLine(line);
            ++idx;
        }

        pages.Add(text.GetCurrentPage(title, color));
        return pages;
    }

    public static PageBuilder GetCurrentPage(this StringBuilder text, string title, Color color) =>
        new PageBuilder()
            .WithDescription(text.ToString())
            .WithColor(color)
            .WithTitle(title);
}
