using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class UnregisteredGuildException : ApiException
{
    public ulong GuildId { get; set; }

    public UnregisteredGuildException(string message, ulong guildId) : base(message, ApiError.GuildUnregistered) =>
        GuildId = guildId;

    public UnregisteredGuildException(ulong guildId) : base($"Guild {guildId} is not registered.",
        ApiError.GuildUnregistered) =>
        GuildId = guildId;
}