using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.AutoMods.Exceptions;

public class InvalidAutoModActionException : ApiException
{
	public InvalidAutoModActionException() : base("Invalid auto mod action.", ApiError.InvalidAutoModAction)
	{
	}
}