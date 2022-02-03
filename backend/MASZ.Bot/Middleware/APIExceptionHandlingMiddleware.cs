using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace MASZ.Bot.Middleware;

public class ApiExceptionHandlingMiddleware
{
	private readonly ILogger _logger;
	private readonly RequestDelegate _next;

	public ApiExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
	{
		_next = next;
		_logger = loggerFactory.CreateLogger<ApiExceptionHandlingMiddleware>();
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (ApiException ex)
		{
			var message = ex.Message;
			context.Response.StatusCode = 400;

			if (ex.Error is ApiError.InvalidIdentity or ApiError.Unauthorized)
				context.Response.StatusCode = 401;
			else if (ex.Error is ApiError.ResourceNotFound)
				context.Response.StatusCode = 404;

			_logger.LogWarning($"Encountered API error type {ex.Error}, message: " + message);

			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonSerializer.Serialize(new { customMASZError = ex.Error, message }));
		}
	}
}