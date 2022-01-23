using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.UserMaps.Exceptions;

public class InvalidUserMapException : ApiException
{
	public InvalidUserMapException() : base("Cannot create user map for same user.", ApiError.CannotBeSameUser)
	{
	}
}