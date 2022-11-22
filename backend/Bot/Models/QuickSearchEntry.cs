using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Models;

public class QuickSearchEntry<T> : IQuickSearchEntry
{
    public T Entry { get; set; }
    public QuickSearchEntryType QuickSearchEntryType { get; set; }
    public DateTime CreatedAt { get; set; }
}
