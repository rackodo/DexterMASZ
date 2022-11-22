namespace Bot.Dynamics;

public interface IAddChart
{
    public Task AddChartData(dynamic chart, ulong guildId, DateTime since);
}
