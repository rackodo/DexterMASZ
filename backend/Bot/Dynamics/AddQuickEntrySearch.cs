using Bot.Abstractions;

namespace Bot.Dynamics;

public interface IAddQuickEntrySearch
{
    public Task AddQuickSearchResults(List<IQuickSearchEntry> entries, ulong guildId, string search);
}
