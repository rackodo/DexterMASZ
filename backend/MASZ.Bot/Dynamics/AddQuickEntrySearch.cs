using MASZ.Bot.Abstractions;

namespace MASZ.Bot.Dynamics;

public interface AddQuickEntrySearch
{
	public Task AddQuickSearchResults(List<QuickSearchEntry> entries, ulong guildId, string search);
}