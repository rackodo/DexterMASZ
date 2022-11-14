namespace Bot.Dynamics;

public interface AddGuildStats
{
    public Task AddGuildStatistics(dynamic stats, ulong guildId);
}