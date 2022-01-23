using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class ResourceNotFoundException : ApiException
{
	public ResourceNotFoundException(string message) : base(message, ApiError.ResourceNotFound)
	{
	}

	public ResourceNotFoundException() : base("Resource not found.", ApiError.ResourceNotFound)
	{
	}
}