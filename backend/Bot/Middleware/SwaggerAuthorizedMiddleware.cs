using Bot.Services;
using Microsoft.AspNetCore.Http;

namespace Bot.Middleware;

internal class SwaggerAuthorizedMiddleware
{
	private readonly RequestDelegate _next;
	private readonly IdentityManager _identityManager;

	public SwaggerAuthorizedMiddleware(RequestDelegate next, IdentityManager identityManager)
	{
		_next = next;
		_identityManager = identityManager;
	}

	public async Task Invoke(HttpContext context)
	{
		if (context.Request.Path.StartsWithSegments("/swagger"))
		{
			var _identity = await _identityManager.GetIdentity(context);

			if (!await _identity.IsSiteAdmin())
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return;
			}
		}

		await _next.Invoke(context);
	}
}
