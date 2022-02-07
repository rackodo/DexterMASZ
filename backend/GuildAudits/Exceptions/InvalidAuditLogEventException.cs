using Bot.Abstractions;
using Bot.Enums;

namespace GuildAudits.Exceptions;

public class InvalidAuditLogEventException : ApiException
{
	public InvalidAuditLogEventException() : base("Invalid audit log event type.", ApiError.InvalidAuditLogEvent)
	{
	}
}