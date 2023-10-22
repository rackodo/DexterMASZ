using Bot.Abstractions;
using Bot.Enums;
using Punishments.Models;

namespace Punishments.Exceptions;

public class ProtectedModCaseSuspectException(string message, ModCase modCase) : ApiException(message,
    ApiError.ProtectedModCaseSuspect)
{
    public ModCase ModCase { get; set; } = modCase;
}
