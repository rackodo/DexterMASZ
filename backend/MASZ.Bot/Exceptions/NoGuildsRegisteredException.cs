using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class NoGuildsRegisteredException : ApiException
{
	public NoGuildsRegisteredException() : base("No guilds registered", ApiError.NoGuildsRegistered)
	{
	}
}