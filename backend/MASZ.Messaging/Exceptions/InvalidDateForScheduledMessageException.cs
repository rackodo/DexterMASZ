using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Messaging.Exceptions;

public class InvalidDateForScheduledMessageException : ApiException
{
	public InvalidDateForScheduledMessageException(string message) : base(message, ApiError.InvalidDateForScheduledMessage)
	{
	}
	public InvalidDateForScheduledMessageException() : base("The defined execution day of this message is invalid.", ApiError.InvalidDateForScheduledMessage)
	{
	}
}