using Bot.Abstractions;
using Bot.Enums;
using Punishments.Models;

namespace Punishments.Exceptions;

public class ProtectedModCaseSuspectException : ApiException
{
    public ModCase ModCase { get; set; }

    public ProtectedModCaseSuspectException(string message, ModCase modCase) : base(message,
        ApiError.ProtectedModCaseSuspect) =>
        ModCase = modCase;
}
