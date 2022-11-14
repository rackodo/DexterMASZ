using Bot.Abstractions;
using Bot.Enums;

namespace AutoMods.Exceptions;

public class InvalidAutoModActionException : ApiException
{
    public InvalidAutoModActionException() : base("Invalid auto mod action.", ApiError.InvalidAutoModAction)
    {
    }
}