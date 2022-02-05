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

var skipStartup = ConsoleCreator.WaitForUser("skip update setting prompts", 10);

var builder = WebApplication.CreateBuilder();

// DATABASES

ConsoleCreator.AddHeading("Getting Database Info");

var (databaseSettings, hasUpdatedDbSettings) = ConsoleCreator.CreateDatabaseSettings(false);

if (hasUpdatedDbSettings)
{
	ConsoleCreator.AddSubHeading("You are finished creating the database settings for", databaseSettings.User);
}
else
{
	ConsoleCreator.AddSubHeading("Found database settings for", $"{databaseSettings.User} // {databaseSettings.Database}");

	if (!skipStartup)
		if (ConsoleCreator.WaitForUser($"edit {nameof(DatabaseSettings)}", 10))
			databaseSettings = ConsoleCreator.CreateDatabaseSettings(true).Key;

	Console.WriteLine();
}

var clientIdContainer = new ClientIdContainer(databaseSettings.ClientId);

builder.Services.AddSingleton(clientIdContainer);

ConsoleCreator.AddSubHeading("Successfully created", "MySQL database provider");

var connectionString = $"Server={databaseSettings.Host};Port={databaseSettings.Port};" +
	$"Database={databaseSettings.Database};Uid={databaseSettings.User};Pwd={databaseSettings.Pass};";

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

	var appSettingRepo = new SettingsRepository(dataContext, clientIdContainer, null);

	settings = await appSettingRepo.GetAppSettings();

	if (settings is null)
	{
		ConsoleCreator.AddHeading("Running First Time Setup");

		ConsoleCreator.AddSubHeading("Welcome to", "MASZ!");
		ConsoleCreator.AddSubHeading("Support Discord", "https://discord.gg/5zjpzw6h3S");

		settings = ConsoleCreator.CreateAppSettings(clientIdContainer, false);

		await appSettingRepo.AddAppSetting(settings);

		ConsoleCreator.AddSubHeading("You are finished creating the app settings for client",
			databaseSettings.ClientId.ToString());

		ConsoleCreator.AddSubHeading("You can now access the panel at", settings.ServiceBaseUrl);

		ConsoleCreator.AddSubHeading("You can always change these settings", "by pressing any key on next reboot");
	}
	else
	{
		ConsoleCreator.AddSubHeading("Found app settings for client",
			databaseSettings.ClientId.ToString());

		if (!skipStartup)
			if (ConsoleCreator.WaitForUser($"edit {nameof(AppSettings)}", 10))
			{
				settings = ConsoleCreator.CreateAppSettings(clientIdContainer, true);

				await appSettingRepo.UpdateAppSetting(settings);

				Console.WriteLine();
			}
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