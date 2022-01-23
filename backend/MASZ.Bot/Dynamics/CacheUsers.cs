namespace MASZ.Bot.Dynamics;

public interface CacheUsers
{
	public Task CacheKnownUsers(List<ulong> handledUsers);
}