using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Punishments.Models;

namespace MASZ.Punishments.Exceptions;

public class ProtectedModCaseSuspectException : ApiException
{
	public ProtectedModCaseSuspectException(string message, ModCase modCase) : base(message,
		ApiError.ProtectedModCaseSuspect)
	{
		ModCase = modCase;
	}

	public ModCase ModCase { get; set; }
}