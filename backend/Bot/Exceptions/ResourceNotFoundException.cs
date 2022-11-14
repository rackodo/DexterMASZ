using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class ResourceNotFoundException : ApiException
{
    public ResourceNotFoundException(string message) : base(message, ApiError.ResourceNotFound)
    {
    }

    public ResourceNotFoundException() : base("Resource not found.", ApiError.ResourceNotFound)
    {
    }
}