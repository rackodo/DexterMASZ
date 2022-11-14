namespace Bot.Dynamics;

public interface AddChart
{
    public Task AddChartData(dynamic chart, ulong guildId, DateTime since);
}