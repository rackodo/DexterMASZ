using Bot.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Middleware;

public class HeaderMiddleware(RequestDelegate next, IServiceProvider services)
{
    private readonly RequestDelegate _next = next;
    private readonly IServiceProvider _services = services;

    public async Task Invoke(HttpContext context)
    {
        using var scope = _services.CreateScope();
        var config = await scope.ServiceProvider.GetRequiredService<SettingsRepository>().GetAppSettings();
        context.Request.Headers["Host"] = config.ServiceDomain;
        await _next(context);
    }
}
