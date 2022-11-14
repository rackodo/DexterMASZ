using Bot.Abstractions;

namespace Bot.Dynamics;

public interface AddQuickEntrySearch
{
    public Task AddQuickSearchResults(List<QuickSearchEntry> entries, ulong guildId, string search);
}