using Bot.Abstractions;
using Bot.Enums;

namespace Punishments.Exceptions;

public class CaseMarkedToBeDeletedException : ApiException
{
    public CaseMarkedToBeDeletedException() : base("Case is marked to be deleted.", ApiError.ModCaseIsMarkedToBeDeleted)
    {
    }
}
