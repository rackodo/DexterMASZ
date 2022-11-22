namespace Bot.Dynamics;

public interface IDeleteGuildData
{
    public Task DeleteGuildData(ulong guildId);
}
