using Bot.Abstractions;
using Bot.Enums;

namespace Punishments.Exceptions;

public class AlreadyCommentedException : ApiException
{
	public AlreadyCommentedException() : base("Already commented", ApiError.LastCommentAlreadyFromSuspect)
	{
	}
}