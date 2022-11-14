using Bot.Enums;

namespace Bot.Abstractions;

public abstract class ApiException : Exception
{
    public ApiError Error { get; set; }

    protected ApiException(string message, ApiError error) : base(message) => Error = error;

    public Exception WithError(ApiError error)
    {
        Error = error;
        return this;
    }
}