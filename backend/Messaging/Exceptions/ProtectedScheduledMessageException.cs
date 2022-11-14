using Bot.Abstractions;
using Bot.Enums;
using Messaging.Models;

namespace Messaging.Exceptions;

public class ProtectedScheduledMessageException : ApiException
{
    public ScheduledMessage ScheduledMessage { get; set; }

    public ProtectedScheduledMessageException(string message, ScheduledMessage scheduledMessage) : base(message,
        ApiError.ProtectedScheduledMessage) => ScheduledMessage = scheduledMessage;

    public ProtectedScheduledMessageException(ScheduledMessage scheduledMessage) : base("Message is protected.",
        ApiError.ProtectedScheduledMessage) => ScheduledMessage = scheduledMessage;

    public ProtectedScheduledMessageException() : base("Message is protected.", ApiError.ProtectedScheduledMessage)
    {
    }
}