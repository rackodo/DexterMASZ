using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class InvalidPathException : ApiException
{
	public InvalidPathException() : base("Invalid file path provided.", ApiError.InvalidFilePath)
	{
	}
}