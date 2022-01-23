using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Messaging.Models;

namespace MASZ.Messaging.Exceptions;

public class ProtectedScheduledMessageException : ApiException
{
	public ScheduledMessage ScheduledMessage { get; set; }

	public ProtectedScheduledMessageException(string message, ScheduledMessage scheduledMessage) : base(message, ApiError.ProtectedScheduledMessage)
	{
		ScheduledMessage = scheduledMessage;
	}
	public ProtectedScheduledMessageException(ScheduledMessage scheduledMessage) : base("Message is protected.", ApiError.ProtectedScheduledMessage)
	{
		ScheduledMessage = scheduledMessage;
	}
	public ProtectedScheduledMessageException() : base("Message is protected.", ApiError.ProtectedScheduledMessage)
	{
	}
}