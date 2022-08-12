using AspNetCoreRateLimit;
using Bot.Abstractions;
using Bot.Middleware;
using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bot;

public class BotWebModule : WebModule
{
	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] AddAuthorizationPolicy()
	{
		return new[] { "Cookies" };
	}

	public override void ConfigureServices(ConfigurationManager configuration, IServiceCollection services)
	{
		services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

		services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
	}

	public override void AddServices(IServiceCollection services, CachedServices cachedServices, AppSettings settings)
	{
		services.AddSingleton<FilesHandler>();

		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie("Cookies", options =>
			{
				options.LoginPath = "/api/v1/login";
				options.LogoutPath = "/api/v1/logout";
				options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
				options.Cookie.MaxAge = new TimeSpan(7, 0, 0, 0);
				options.Cookie.Name = "dexter_access_token";
				options.Cookie.HttpOnly = false;
				options.Events.OnRedirectToLogin = context =>
				{
					context.Response.Headers["Location"] = context.RedirectUri;
					context.Response.StatusCode = 401;
					return Task.CompletedTask;
				};
			})
			.AddDiscord(options =>
			{
				options.ClientId = settings.ClientId.ToString();
				options.ClientSecret = settings.ClientSecret;
				options.Scope.Add("guilds");
				options.Scope.Add("identify");
				options.SaveTokens = true;
				options.Prompt = "none";
				options.AccessDeniedPath = "/oauthfailed";
				options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.CorrelationCookie.SameSite = SameSiteMode.Lax;
				options.CorrelationCookie.HttpOnly = false;
			});

		if (settings.CorsEnabled)
			services.AddCors(o => o.AddPolicy("AngularDevCors", builder =>
			{
				builder.WithOrigins("http://127.0.0.1:4200")
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials();
			}));

		services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
		services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
		services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
		services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	public override void PostWebBuild(WebApplication app, AppSettings settings)
	{
		if (settings.CorsEnabled)
			app.UseCors("AngularDevCors");

		if (app.Environment.IsDevelopment())
			app.UseDeveloperExceptionPage();

		app.UseIpRateLimiting();

		app.UseMiddleware<HeaderMiddleware>();
		app.UseMiddleware<RequestLoggingMiddleware>();
		app.UseMiddleware<ApiExceptionHandlingMiddleware>();
		app.UseMiddleware<SwaggerAuthorizedMiddleware>();

		app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			options.RoutePrefix = string.Empty;
		});
	}
}