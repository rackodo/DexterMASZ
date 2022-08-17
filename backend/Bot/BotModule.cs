using Bot.Abstractions;
using Bot.Loggers;
using Bot.Models;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot;

public class BotModule : Module
{
	public override string[] Contributors { get; } = { "Zaanposni", "Ferox" };

	public override void AddLogging(ILoggingBuilder loggingBuilder)
	{
		loggingBuilder.ClearProviders();
		loggingBuilder.AddProvider(new LoggerProvider());
	}

	public override void AddPreServices(IServiceCollection services, CachedServices cachedServices,
		Action<DbContextOptionsBuilder> dbOption)
	{
		foreach (var type in cachedServices.GetClassTypes<DataContextInitialize>())
			type.GetMethod("AddContextToServiceProvider")
				.Invoke(null, new object[] { dbOption, services });

		foreach (var type in cachedServices.GetClassTypes<Repository>())
			services.AddScoped(type);

		services.AddScoped<Translation>();

		foreach (var type in cachedServices.GetClassTypes<Translator>())
			services.AddScoped(type);
	}

	public override void AddServices(IServiceCollection services, CachedServices cachedServices, AppSettings settings)
	{
		services.AddSingleton(new DiscordSocketConfig
		{
			AlwaysDownloadUsers = true,
			MessageCacheSize = 10240,
			LogLevel = LogSeverity.Debug,
			GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
			LogGatewayIntentWarnings = false
		});

		services.AddSingleton<DiscordSocketClient>();

		services.AddSingleton(new InteractionServiceConfig
		{
			DefaultRunMode = RunMode.Async,
			LogLevel = LogSeverity.Debug,
			UseCompiledLambda = true
		});

		services.AddSingleton<InteractionService>();

		foreach (var type in cachedServices.GetClassTypes<InternalEventHandler>())
			services.AddSingleton(type);

		foreach (var type in cachedServices.GetClassTypes<Event>())
			services.AddSingleton(type);

		services.AddHostedService(s => s.GetRequiredService<DiscordBot>());
		services.AddHostedService(s => s.GetRequiredService<AuditLogger>());
		services.AddHostedService(s => s.GetRequiredService<DiscordRest>());
	}

	public override void PostBuild(IServiceProvider services, CachedServices cachedServices)
	{
		foreach (var initEvent in cachedServices.GetInitializedClasses<Event>(services))
			initEvent.RegisterEvents();
	}
}