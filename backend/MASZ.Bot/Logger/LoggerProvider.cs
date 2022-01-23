using Microsoft.Extensions.Logging;

namespace MASZ.Bot.Logger;

public class LoggerProvider : ILoggerProvider
{
	public ILogger CreateLogger(string categoryName)
	{
		return new ConsoleLogger(categoryName);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}
}