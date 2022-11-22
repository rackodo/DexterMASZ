using Bot.Abstractions;
using Bot.Enums;

namespace Punishments.Exceptions;

public class AlreadyFinalWarnedException : ApiException
{
    public AlreadyFinalWarnedException()
        : base("This user already has a final warning applied!", ApiError.AlreadyFinalWarned)
    {
    }
}
