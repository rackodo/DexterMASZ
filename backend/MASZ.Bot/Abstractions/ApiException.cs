using MASZ.Bot.Enums;

namespace MASZ.Bot.Abstractions;

public abstract class ApiException : Exception
{
	protected ApiException(string message, ApiError error) : base(message)
	{
		Error = error;
	}

	public ApiError Error { get; set; }

	public Exception WithError(ApiError error)
	{
		Error = error;
		return this;
	}
}