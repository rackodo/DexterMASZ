using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Models;

public class QuickSearchEntry<T> : QuickSearchEntry
{
	public T Entry { get; set; }
	public QuickSearchEntryType QuickSearchEntryType { get; set; }
	public DateTime CreatedAt { get; set; }
}