using Fergun.Interactive;
using System.Text;

namespace Music.Extensions;

internal class MusicPages
{
    public static IEnumerable<PageBuilder> CreatePagesFromString(StringBuilder content, int fixedPageSplit = 15,
        int threshold = 2000)
    {
        var lines = content.ToString()
            .Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var idx = 0;
        List<PageBuilder> pages = new();
        StringBuilder text = new();

        foreach (var line in lines)
        {
            if (idx != 0 && idx % fixedPageSplit == 0 || text.Length + line.Length > threshold)
            {
                pages.Add(new PageBuilder().WithText(text.ToString()));
                text.Clear();
            }

            text.AppendLine(line);
            ++idx;
        }

        pages.Add(new PageBuilder().WithText(text.ToString()));
        return pages;
    }
}
