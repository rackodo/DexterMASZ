using Bot.Abstractions;
using Bot.Enums;

namespace AutoMods.Exceptions;

public class InvalidAutoModTypeException : ApiException
{
    public InvalidAutoModTypeException() : base("Invalid auto mod type.", ApiError.InvalidAutoModType)
    {
    }
}
