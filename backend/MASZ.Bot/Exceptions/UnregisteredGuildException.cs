using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Exceptions;

public class UnregisteredGuildException : ApiException
{
	public UnregisteredGuildException(string message, ulong guildId) : base(message, ApiError.GuildUnregistered)
	{
		GuildId = guildId;
	}

	public UnregisteredGuildException(ulong guildId) : base($"Guild {guildId} is not registered.",
		ApiError.GuildUnregistered)
	{
		GuildId = guildId;
	}
	
	public ulong GuildId { get; set; }
}