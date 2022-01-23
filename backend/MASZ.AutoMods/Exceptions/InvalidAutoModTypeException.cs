using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.AutoMods.Exceptions;

public class InvalidAutoModTypeException : ApiException
{
	public InvalidAutoModTypeException() : base("Invalid auto mod type.", ApiError.InvalidAutoModerationType)
	{
	}
}