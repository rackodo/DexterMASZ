using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class UnauthorizedException : ApiException
{
	public UnauthorizedException(string message) : base(message, ApiError.Unauthorized)
	{
	}

	public UnauthorizedException() : base("You are not allowed to do that", ApiError.Unauthorized)
	{
	}
}