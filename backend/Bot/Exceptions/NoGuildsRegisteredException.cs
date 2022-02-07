using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class NoGuildsRegisteredException : ApiException
{
	public NoGuildsRegisteredException() : base("No guilds registered", ApiError.NoGuildsRegistered)
	{
	}
}