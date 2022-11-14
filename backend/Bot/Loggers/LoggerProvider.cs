using Microsoft.Extensions.Logging;

namespace Bot.Loggers;

public class LoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new ConsoleLogger(categoryName);

    public void Dispose() => GC.SuppressFinalize(this);
}