using System.Globalization;
using Bot.Models;

namespace Bot.Extensions;

public static class SearchContains
{
    public static bool Search(this string search, string obj) => !string.IsNullOrWhiteSpace(obj) &&
                                                                 obj.Contains(search,
                                                                     StringComparison.CurrentCultureIgnoreCase);

    public static bool Search(this string search, ulong obj) => search.Search(obj.ToString());

    public static bool Search(this string search, DateTime obj) =>
        obj != default && search.Search(obj.ToString(CultureInfo.CurrentCulture));

    public static bool Search(this string search, string[] obj) => obj != null && obj.Any(search.Search);

    public static bool Search(this string search, DiscordUser obj) =>
        obj != null && search.Search($"{obj.Username}#{obj.Discriminator}");
}