using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class ResourceAlreadyExists : ApiException
{
	public ResourceAlreadyExists() : base("Resource already exists.", ApiError.ResourceAlreadyExists)
	{
	}
}