using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class InvalidPathException : ApiException
{
    public InvalidPathException() : base("Invalid file path provided.", ApiError.InvalidFilePath)
    {
    }
}