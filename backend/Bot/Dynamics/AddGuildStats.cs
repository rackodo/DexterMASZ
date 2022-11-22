namespace Bot.Dynamics;

public interface IAddGuildStats
{
    public Task AddGuildStatistics(dynamic stats, ulong guildId);
}
