namespace Bot.Dynamics;

public interface IAddNetworks
{
    public Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId);
}
