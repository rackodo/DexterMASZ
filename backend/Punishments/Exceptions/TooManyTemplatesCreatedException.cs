using Bot.Abstractions;
using Bot.Enums;

namespace Punishments.Exceptions;

public class TooManyTemplatesCreatedException : ApiException
{
    public TooManyTemplatesCreatedException() : base("Too many templates created.", ApiError.TooManyTemplates)
    {
    }
}