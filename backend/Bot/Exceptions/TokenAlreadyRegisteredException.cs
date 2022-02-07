using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class TokenAlreadyRegisteredException : ApiException
{
	public TokenAlreadyRegisteredException() : base("There already is a token.", ApiError.TokenAlreadyRegistered)
	{
	}
}