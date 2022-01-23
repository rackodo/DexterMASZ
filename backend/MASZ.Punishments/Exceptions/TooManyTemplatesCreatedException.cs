using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Punishments.Exceptions;

public class TooManyTemplatesCreatedException : ApiException
{
	public TooManyTemplatesCreatedException() : base("Too many templates created.", ApiError.TooManyTemplates)
	{
	}
}