using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.GuildAudits.Exceptions;

public class InvalidAuditLogEventException : ApiException
{
	public InvalidAuditLogEventException() : base("Invalid audit log event type.", ApiError.InvalidAuditLogEvent)
	{
	}
}