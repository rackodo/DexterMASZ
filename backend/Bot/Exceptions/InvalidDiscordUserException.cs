using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class InvalidIUserException : ApiException
{
	public InvalidIUserException(string message, ulong userId) : base(message, ApiError.InvalidDiscordUser)
	{
		UserId = userId;
	}

	public InvalidIUserException(ulong userId) : base("Failed to fetch user '{userId}' from API.",
		ApiError.InvalidDiscordUser)
	{
		UserId = userId;
	}

	public ulong UserId { get; set; }
}