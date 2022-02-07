using Bot.Abstractions;
using Bot.Enums;

namespace UserMaps.Exceptions;

public class InvalidUserMapException : ApiException
{
	public InvalidUserMapException() : base("Cannot create user map for same user.", ApiError.CannotBeSameUser)
	{
	}
}