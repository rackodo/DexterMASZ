using Bot.Abstractions;
using Bot.Enums;
using Punishments.Models;

namespace Punishments.Exceptions;

public class ProtectedModCaseSuspectException : ApiException
{
	public ProtectedModCaseSuspectException(string message, ModCase modCase) : base(message,
		ApiError.ProtectedModCaseSuspect)
	{
		ModCase = modCase;
	}

	public ModCase ModCase { get; set; }
}