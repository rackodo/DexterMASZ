namespace MASZ.Bot.Dynamics;

public interface AddSearch
{
	public Task AddSearchData(dynamic data, ulong guildId, string search);
}