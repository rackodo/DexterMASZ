using Launch;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("========== Launching MASZ ==========");

var builder = WebApplication.CreateBuilder();

// DATABASES

ConsoleCreator.AddHeading("Getting Database Info");

var server = ConsoleCreator.AskAndSet("server host", "MYSQL_HOST");

var port = ConsoleCreator.AskAndSet("server port", "MYSQL_PORT");

var database = ConsoleCreator.AskAndSet("database name", "MYSQL_DATABASE");

var uid = ConsoleCreator.AskAndSet("login username", "MYSQL_USER");

var pwd = ConsoleCreator.AskAndSet("login password", "MYSQL_PASSWORD");

ConsoleCreator.AddSubHeading("Successfully created", "MySQL database provider");

var connectionString = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={pwd};";

Action<DbContextOptionsBuilder> databaseBuilder = x => x.UseMySql(
	connectionString,
	ServerVersion.AutoDetect(connectionString),
	o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}")
);

// APP SETTINGS

ConsoleCreator.AddHeading($"Getting {nameof(AppSettings)}");

var dbBuilder = new DbContextOptionsBuilder<BotDatabase>();

databaseBuilder.Invoke(dbBuilder);

AppSettings settings;

ConsoleCreator.AddSubHeading("Querying database for", nameof(AppSettings));

await using (var dataContext = new BotDatabase(dbBuilder.Options))
{
	await dataContext.Database.MigrateAsync();

	var clientId = ulong.Parse(ConsoleCreator.AskAndSet("Discord OAuth Client ID", "DISCORD_OAUTH_CLIENT_ID"));

	var clientIdContainer = new ClientIdContainer(clientId);

	builder.Services.AddSingleton(clientIdContainer);

	var appSettingRepo = new SettingsRepository(dataContext, clientIdContainer, null);

	settings = await appSettingRepo.GetAppSettings();

	if (settings is null)
	{
		ConsoleCreator.AddHeading("Running First Time Setup");

		ConsoleCreator.AddSubHeading("Welcome to", "MASZ!");
		ConsoleCreator.AddSubHeading("Support Discord", "https://discord.gg/5zjpzw6h3S");

		settings = ConsoleCreator.CreateAppSettings(clientId, false);

		await appSettingRepo.AddAppSetting(settings);

		ConsoleCreator.AddSubHeading("You are finished creating app settings for client", clientId.ToString());

		ConsoleCreator.AddSubHeading("You can now access the panel at", settings.ServiceBaseUrl);

		ConsoleCreator.AddSubHeading("You can always change these settings", "by pressing any key on next reboot");
	}
	else
	{
		ConsoleCreator.AddSubHeading("Found app settings for client", clientId.ToString());

		var original = DateTime.Now;
		var newTime = original;

		var waitTime = 10;
		var remainingWaitTime = waitTime;
		var lastWaitTime = waitTime.ToString();
		var keyRead = false;

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.Write($"Press any key to edit {nameof(AppSettings)} before: ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write(waitTime);

		do
		{
			keyRead = Console.KeyAvailable;
			if (!keyRead)
			{
				newTime = DateTime.Now;
				remainingWaitTime = waitTime - (int)(newTime - original).TotalSeconds;
				var newWaitTime = remainingWaitTime.ToString();
				if (newWaitTime != lastWaitTime)
				{
					var backSpaces = new string('\b', lastWaitTime.Length);
					var spaces = new string(' ', lastWaitTime.Length);
					Console.Write(backSpaces + spaces + backSpaces);
					lastWaitTime = newWaitTime;
					Console.Write(lastWaitTime);
					Thread.Sleep(25);
				}
			}
			else
				Console.ReadKey();
		} while (remainingWaitTime > 0 && !keyRead);

		if (keyRead)
		{
			Console.WriteLine();
			settings = ConsoleCreator.CreateAppSettings(clientId, true);
			await appSettingRepo.UpdateAppSetting(settings);
		}

		Console.WriteLine();
	}
}

// MODULE IMPORTS

var modules = ImportModules.GetModules();

ConsoleCreator.AddHeading("Importing Modules");

foreach (var module in modules)
{
	ConsoleCreator.AddSubHeading("Imported", $"{module.GetType().Namespace}{(module is WebModule ? " (WEB)" : "")}");

	Console.Write("    Maintained By:      ");

	Console.ForegroundColor = ConsoleColor.Magenta;
	Console.WriteLine(module.Maintainer);
	Console.ResetColor();

	if (module.Contributors.Length > 0)
	{
		Console.Write("    Contributions From");

		Console.ForegroundColor = ConsoleColor.DarkMagenta;
		Console.WriteLine(string.Join(", ", module.Contributors));
	}

	Console.ResetColor();

	if (module.Translators.Length > 0)
	{
		Console.Write("    Translated By:      ");

		Console.ForegroundColor = ConsoleColor.DarkMagenta;
		Console.WriteLine(string.Join(", ", module.Translators));
	}
}

Console.ResetColor();

// START WEB SERVER

ConsoleCreator.AddHeading("Starting MASZ");

builder.WebHost.CaptureStartupErrors(true);

builder.WebHost.UseUrls("http://0.0.0.0:80/");

var cachedServices = new CachedServices();

builder.Services.AddSingleton(cachedServices);

var authorizationPolicies = new List<string>();

try
{
	foreach (var startup in modules)
		startup.AddLogging(builder.Logging);

	ConsoleCreator.AddSubHeading("Successfully initialized", "logging");

	foreach (var startup in modules)
		startup.AddPreServices(builder.Services, cachedServices, databaseBuilder);

	ConsoleCreator.AddSubHeading("Successfully initialized", "pre-services");

	foreach (var startup in modules)
		startup.AddServices(builder.Services, cachedServices, settings);
	ConsoleCreator.AddSubHeading("Successfully initialized", "services");

	foreach (var startup in modules)
		startup.ConfigureServices(builder.Configuration, builder.Services);
	ConsoleCreator.AddSubHeading("Successfully configured", "services");

	foreach (var startup in modules)
		if (startup is WebModule module)
		{
			var authorizationPolicy = module.AddAuthorizationPolicy();

			if (authorizationPolicy.Length > 0)
				authorizationPolicies = authorizationPolicies.Union(authorizationPolicy).ToList();
		}

	ConsoleCreator.AddSubHeading("Successfully added", "authentication policies");
}
catch (Exception ex)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(ex.ToString());
	return;
}

builder.Services.AddMemoryCache();

var controller = builder.Services.AddControllers()
	.AddNewtonsoftJson(x =>
	{
		x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
		x.SerializerSettings.Converters.Add(new UlongConverter());
	});

foreach (var assembly in cachedServices.Dependents)
	controller.AddApplicationPart(assembly);

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder(authorizationPolicies.ToArray())
		.RequireAuthenticatedUser().Build();
});

var app = builder.Build();

// MIGRATIONS

ConsoleCreator.AddHeading("Adding Migrations");
Console.WriteLine("This might take a while on a first install...\n");

using (var scope = app.Services.CreateScope())
{
	foreach (var dataContext in cachedServices.GetInitializedClasses<DbContext>(scope.ServiceProvider))
	{
		ConsoleCreator.AddSubHeading("Adding migrations for", dataContext.GetType().Name);
		await dataContext.Database.MigrateAsync();
	}
}

ConsoleCreator.AddSubHeading("Successfully added", "migrations to databases");

// CONFIGURE

ConsoleCreator.AddHeading("Building MASZ");

foreach (var startup in modules)
{
	startup.PostBuild(app.Services, cachedServices);

	if (startup is WebModule module)
		module.PostWebBuild(app, settings);
}

ConsoleCreator.AddSubHeading("Successfully post built", "application");

if (settings.ServiceHostName.Contains("https"))
	app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

if (authorizationPolicies.Any())
	app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});

ConsoleCreator.AddHeading("Running MASZ");

try
{
	await app.RunAsync();
}
catch (Exception ex)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(ex.ToString());
}