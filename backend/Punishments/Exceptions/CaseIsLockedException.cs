using Bot.Abstractions;
using Bot.Enums;

namespace Punishments.Exceptions;

public class CaseIsLockedException : ApiException
{
	public CaseIsLockedException() : base("Case is locked.", ApiError.ModCaseDoesNotAllowComments)
	{
	}
}