namespace Bot.Extensions;

public static class LanguageHelper
{
	private static readonly Dictionary<long, string> BasicUnits = new()
	{
		{ 1000000000000, "T" },
		{ 1000000000, "B" },
		{ 1000000, "M" },
		{ 1000, "K" }
	};

	public static readonly Dictionary<long, string> MetricPrefixes = new()
	{
		{ 1000000000000, "T" },
		{ 1000000000, "G" },
		{ 1000000, "M" },
		{ 1000, "K" }
	};

	public static readonly Dictionary<long, string> ByteUnits = new()
	{
		{ 1099511627776, "TB" },
		{ 1073741824, "GB" },
		{ 1048576, "MB" },
		{ 1024, "KB" }
	};

	public static string ToUnit(this long v, Dictionary<long, string> units = null)
	{
		if (units is null) units = BasicUnits;
		foreach (var kvp in units)
		{
			if (v >= kvp.Key)
			{
				return $"{(float)v / kvp.Key:G3}{kvp.Value}";
			}
		}
		return v.ToString();
	}
}
