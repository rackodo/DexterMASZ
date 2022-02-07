using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class TokenCannotManageException : ApiException
{
	public TokenCannotManageException() : base("Tokens cannot manage this resource.",
		ApiError.TokenCannotManageThisResource)
	{
	}
}