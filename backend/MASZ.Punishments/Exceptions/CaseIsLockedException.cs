using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Punishments.Exceptions;

public class CaseIsLockedException : ApiException
{
	public CaseIsLockedException() : base("Case is locked.", ApiError.ModCaseDoesNotAllowComments)
	{
	}
}