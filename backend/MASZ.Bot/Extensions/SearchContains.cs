using System.Globalization;
using MASZ.Bot.Views;

namespace MASZ.Bot.Extensions;

public static class SearchContains
{
	public static bool Search(this string search, string obj)
	{
		return !string.IsNullOrWhiteSpace(obj) && obj.Contains(search, StringComparison.CurrentCultureIgnoreCase);
	}

	public static bool Search(this string search, ulong obj)
	{
		return search.Search(obj.ToString());
	}

	public static bool Search(this string search, DateTime obj)
	{
		return obj != default && search.Search(obj.ToString(CultureInfo.CurrentCulture));
	}

	public static bool Search(this string search, string[] obj)
	{
		return obj != null && obj.Any(search.Search);
	}
	
	public static bool Search(this string search, DiscordUserView obj)
	{
		return obj != null && search.Search($"{obj.Username}#{obj.Discriminator}");
	}
}