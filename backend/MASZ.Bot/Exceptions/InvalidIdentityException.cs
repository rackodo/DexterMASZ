using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

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