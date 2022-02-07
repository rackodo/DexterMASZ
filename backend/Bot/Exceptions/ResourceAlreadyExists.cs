using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class ResourceAlreadyExists : ApiException
{
	public ResourceAlreadyExists() : base("Resource already exists.", ApiError.ResourceAlreadyExists)
	{
	}
}