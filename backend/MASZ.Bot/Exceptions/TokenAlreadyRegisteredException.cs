using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class TokenAlreadyRegisteredException : ApiException
{
	public TokenAlreadyRegisteredException() : base("There already is a token.", ApiError.TokenAlreadyRegistered)
	{
	}
}