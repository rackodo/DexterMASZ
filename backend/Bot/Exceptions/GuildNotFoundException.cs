using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class GuildNotFoundException : ApiException
{
    public GuildNotFoundException(string message) : base(message, ApiError.GuildNotFound)
    {
    }

    public GuildNotFoundException() : base("Guild not found.", ApiError.GuildNotFound)
    {
    }
}
