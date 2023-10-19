using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class GuildAlreadyRegisteredException(ulong guildId) : ApiException($"Guild {guildId} is already registered.",
    ApiError.GuildUnregistered)
{
    public ulong GuildId { get; set; } = guildId;
}
