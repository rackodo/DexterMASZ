using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Punishments.Exceptions;

public class AlreadyCommentedException : ApiException
{
	public AlreadyCommentedException() : base("Already commented", ApiError.LastCommentAlreadyFromSuspect)
	{
	}
}