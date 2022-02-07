using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bot.Middleware;

public class RequestLoggingMiddleware
{
	private readonly ILogger _logger;
	private readonly RequestDelegate _next;

	public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
	{
		_next = next;
		_logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
	}

	private string GetIp(HttpContext context)
	{
		try
		{
			string ip = context.Request.Headers["X-Forwarded-For"];

			if (string.IsNullOrEmpty(ip))
				ip = context.Request.Headers["REMOTE_ADDR"];
			else
				// Using X-Forwarded-For last address
				ip = ip.Split(',')
					.Last()
					.Trim();

			if (context.Connection.RemoteIpAddress != null)
				return string.IsNullOrEmpty(ip) ? context.Connection.RemoteIpAddress.ToString() : ip;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error getting IP");
		}

		return string.Empty;
	}

	public async Task Invoke(HttpContext context)
	{
		var method = context.Request.Method;

		method = method switch
		{
			"DELETE" => "DEL",
			"OPTIONS" => "OPT",
			"PATCH" => "PAT",
			"TRACE" => "TRC",
			"CONNECT" => "CON",
			"HEAD" => "HED",
			"POST" => "POS",
			_ => method
		};

		try
		{
			_logger.LogInformation(
				$"INC {method} {context.Request.Path.Value}{context.Request.QueryString} | {GetIp(context)}");
			await _next(context);
			_logger.LogInformation($"{context.Response.StatusCode} {method} {context.Request.Path.Value}");
		}
		catch (Exception e)
		{
			_logger.LogError(e, $"500 {method} {context.Request.Path.Value}");
			throw;
		}
	}
}