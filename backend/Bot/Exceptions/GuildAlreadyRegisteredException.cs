using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Exceptions;

public class GuildAlreadyRegisteredException : ApiException
{
	public GuildAlreadyRegisteredException(ulong guildId) : base($"Guild {guildId} is already registered.",
		ApiError.GuildUnregistered)
	{
		GuildId = guildId;
	}

	public ulong GuildId { get; set; }
}