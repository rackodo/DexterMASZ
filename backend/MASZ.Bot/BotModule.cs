using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Logger;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASZ.Bot;

public class BotModule : Module
{
	public override string Maintainer => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster", "FlixProd" };

	public override void AddLogging(ILoggingBuilder loggingBuilder)
	{
		loggingBuilder.ClearProviders();
		loggingBuilder.AddProvider(new LoggerProvider());
	}

	public override void AddPreServices(IServiceCollection services, ServiceCacher serviceCacher,
		Action<DbContextOptionsBuilder> dbOption)
	{
		foreach (var type in serviceCacher.GetClassTypes<DataContextCreate>())
			type.GetMethod("AddContextToServiceProvider")?.Invoke(null, new object[] { dbOption, services });

		foreach (var type in serviceCacher.GetClassTypes<Repository>())
			services.AddScoped(type);

		services.AddScoped<Translation>();

		foreach (var type in serviceCacher.GetClassTypes<Translator>())
			services.AddScoped(type);
	}

	public override void AddServices(IServiceCollection services, ServiceCacher serviceCacher, AppSettings settings)
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

		foreach (var type in serviceCacher.GetClassTypes<Abstractions.EventHandler>())
			services.AddSingleton(type);

		foreach (var type in serviceCacher.GetClassTypes<Event>())
			services.AddSingleton(type);

		services.AddHostedService(s => s.GetRequiredService<DiscordBot>());
		services.AddHostedService(s => s.GetRequiredService<AuditLogger>());
		services.AddHostedService(s => s.GetRequiredService<DiscordRest>());
	}

	public override void PostBuild(IServiceProvider services, ServiceCacher serviceCacher)
	{
		foreach (var initEvent in serviceCacher.GetInitializedClasses<Event>(services))
			initEvent.RegisterEvents();
	}
}