namespace Bot.Dynamics;

public interface IAddSearch
{
    public Task AddSearchData(dynamic data, ulong guildId, string search);
}
