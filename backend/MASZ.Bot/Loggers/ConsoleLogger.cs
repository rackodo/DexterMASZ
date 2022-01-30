using Microsoft.Extensions.Logging;

namespace MASZ.Bot.Loggers;

public class ConsoleLogger : ILogger
{
	private const string DNetClientPrefix = "Discord.WebSocket.DiscordSocketClient";
	private const string DNetPrefix = "Discord.";
	private const LogLevel Level = LogLevel.Information;
	private const string MaszPrefix = "MASZ.";

	private string _categoryName;

	public ConsoleLogger(string categoryName)
	{
		_categoryName = categoryName;
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
		Func<TState, Exception, string> formatter)
	{
		if (!IsEnabled(logLevel))
			return;

		var message = formatter(state, exception);

		string shortLogLevel;

		switch (logLevel)
		{
			case LogLevel.Trace:
				shortLogLevel = "T";
				Console.ForegroundColor = ConsoleColor.DarkGray;
				break;
			case LogLevel.Debug:
				shortLogLevel = "D";
				Console.ForegroundColor = ConsoleColor.DarkGray;
				break;
			case LogLevel.Information:
				shortLogLevel = "I";
				Console.ForegroundColor = ConsoleColor.White;
				break;
			case LogLevel.Warning:
				shortLogLevel = "W";
				Console.ForegroundColor = ConsoleColor.Yellow;
				break;
			case LogLevel.Error:
				shortLogLevel = "E";
				Console.ForegroundColor = ConsoleColor.Red;
				break;
			case LogLevel.Critical:
				shortLogLevel = "C";
				Console.ForegroundColor = ConsoleColor.Magenta;
				break;
			default:
				shortLogLevel = "N";
				Console.ForegroundColor = ConsoleColor.Gray;
				break;
		}

		if (_categoryName.StartsWith(MaszPrefix))
			_categoryName = _categoryName.Split('.').Last()
				.Replace("RequestLoggingMiddleware", "ReqLog")
				.Replace("Command", "Cmd")
				.Replace("Interface", "I");

		else if (_categoryName.StartsWith(DNetClientPrefix))
			_categoryName = _categoryName.Replace(DNetClientPrefix, "DNET.Client");

		else if (_categoryName.StartsWith(DNetPrefix))
			_categoryName = _categoryName.Replace(DNetPrefix, "DNET.");

		var currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
		var prefix = $"[{currentTime}] [{shortLogLevel}] {_categoryName}[{eventId.Id}]: ";

		Console.WriteLine($"{prefix}{message}");

		if (exception != null)
		{
			Console.WriteLine(exception.Message);

			if (exception.StackTrace != null)
				Console.WriteLine(exception.StackTrace);
		}

		Console.ResetColor();
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return logLevel >= Level;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return null;
	}
}