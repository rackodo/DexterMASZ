using Bot.Abstractions;
using Bot.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bot.Middleware;

public class ApiExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ApiExceptionHandlingMiddleware>();
    private readonly RequestDelegate _next = next;

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
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { customDexterError = ex.Error, message }));
        }
    }
}
