using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class TokenCannotManageException : ApiException
{
	public TokenCannotManageException() : base("Tokens cannot manage this resource.",
		ApiError.TokenCannotManageThisResource)
	{
	}
}