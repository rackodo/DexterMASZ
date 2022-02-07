using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class InvalidIdentityException : ApiException
{
	public InvalidIdentityException(string token) : base("Invalid identity (token) encountered.",
		ApiError.InvalidIdentity)
	{
		Token = token;
	}

	public InvalidIdentityException() : base("Invalid identity (token) encountered.", ApiError.InvalidIdentity)
	{
	}

	public string Token { get; set; }
}