using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Punishments.Exceptions;

public class CaseMarkedToBeDeletedException : ApiException
{
	public CaseMarkedToBeDeletedException() : base("Case is marked to be deleted.", ApiError.ModCaseIsMarkedToBeDeleted)
	{
	}
}