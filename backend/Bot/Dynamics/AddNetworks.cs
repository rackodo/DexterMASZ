namespace Bot.Dynamics;

public interface AddNetworks
{
    public Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId);
}