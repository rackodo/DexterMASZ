using Launch;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Enums;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Reflection;

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("========== Launching MASZ ==========");

// DATABASES

ConsoleCreator.AddHeading("Getting Database Info");

var server = ConsoleCreator.AskAndSet("server host", "MYSQL_HOST");

var port = ConsoleCreator.AskAndSet("server port", "MYSQL_PORT");

var database = ConsoleCreator.AskAndSet("database name", "MYSQL_DATABASE");

var uid = ConsoleCreator.AskAndSet("login username", "MYSQL_USER");

var pwd = ConsoleCreator.AskAndSet("login password", "MYSQL_PASSWORD");

ConsoleCreator.AddSubHeading("Successfully created: ", "MySQL Database Provider");

var connectionString = $"Server={server};Port={port};Database={database};Uid={uid};Pwd={pwd};";

Action<DbContextOptionsBuilder> databaseBuilder = x => x.UseMySql(
	connectionString,
	ServerVersion.AutoDetect(connectionString),
	o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}")
);

// APP SETTINGS

ConsoleCreator.AddHeading("Getting App Settings");

var dbBuilder = new DbContextOptionsBuilder<BotDatabase>();

databaseBuilder.Invoke(dbBuilder);

AppSettings settings;

ConsoleCreator.AddSubHeading("Querying Database For: ", "App Settings");

await using (var dataContext = new BotDatabase(dbBuilder.Options))
{
	await dataContext.Database.MigrateAsync();

	var appSettingRepo = new SettingsRepository(dataContext, null);

	settings = await appSettingRepo.GetAppSettings();

	if (settings is null)
	{
		settings = new AppSettings
		{
			ClientId = ulong.Parse(ConsoleCreator.Ask("Discord OAuth Client ID", "DISCORD_OAUTH_CLIENT_ID")),
			ClientSecret = ConsoleCreator.Ask("Discord OAuth Client Secret", "DISCORD_OAUTH_CLIENT_SECRET"),

			DiscordBotToken = ConsoleCreator.Ask("Discord bot token", "DISCORD_BOT_TOKEN"),
			AuditLogWebhookUrl = ConsoleCreator.Ask(
				"audit log webhook url, recommended to be in a private channel for site admins " +
				"as it may log sensitive information, or leave empty to disable", "AUDIT_LOG_WEBHOOK_URL"),

			Lang = Enum.GetName(
				ConsoleCreator.AskDefinedChoice<Language>("default language", "DEFAULT_LANGUAGE", false)),

			PublicFileMode = ConsoleCreator.AskDefinedChoice<Booleans>("stance on whether files should be public",
				"ENABLE_PUBLIC_FILES", false) == Booleans.True,
			CorsEnabled = ConsoleCreator.AskDefinedChoice<Booleans>(
				"stance on whether this is in a development environment",
				"ENABLE_CORS", false) == Booleans.True,
			DemoModeEnabled = ConsoleCreator.AskDefinedChoice<Booleans>(
				"stance on whether this is being used as a demonstration",
				"ENABLE_DEMO_MODE", false) == Booleans.True
		};

		var admins =
			ConsoleCreator.Ask("site administrator ids, recommended as just one, but can be split by ','",
				"DISCORD_SITE_ADMINS");

		if (!string.IsNullOrEmpty(admins))
			settings.SiteAdmins = admins.Split(',').Select(ulong.Parse).ToArray();

		var directoryPath = ConsoleCreator.Ask("directory for files to be saved (leave empty for current)",
			"ABSOLUTE_PATH_TO_FILE_UPLOAD");

		if (string.IsNullOrEmpty(directoryPath))
			directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		settings.AbsolutePathToFileUpload = directoryPath;

		switch (ConsoleCreator.AskDefinedChoice<DeploymentType>("stance on whether this is being deployed on " +
																"a domain or locally, as a test version", "DEPLOY_MODE",
					false))
		{
			case DeploymentType.Domain:
				settings.ServiceHostName = ConsoleCreator.Ask("service name", "META_SERVICE_NAME");
				settings.ServiceDomain = ConsoleCreator.Ask("(sub)domain", "META_SERVICE_DOMAIN");
				settings.ServiceBaseUrl = $"https://{settings.ServiceDomain}";
				break;
			case DeploymentType.Local:
				settings.ServiceHostName = ConsoleCreator.Ask("service name", "META_SERVICE_NAME");
				settings.ServiceDomain = "127.0.0.1:5565";
				settings.ServiceBaseUrl = $"http://{settings.ServiceDomain}";
				break;
			default:
				throw new NotImplementedException();
		}

		await appSettingRepo.AddAppSetting(settings);

		ConsoleCreator.AddSubHeading("Created app settings for client: ", settings.ClientId.ToString());
	}
	else
	{
		ConsoleCreator.AddSubHeading("Found app settings for client: ", settings.ClientId.ToString());
	}
}

// MODULE IMPORTS

var modules = ImportModules.GetModules();

ConsoleCreator.AddHeading("Importing Modules");

foreach (var module in modules)
{
	ConsoleCreator.AddSubHeading("Imported: ", $"{module.GetType().Namespace}{(module is WebModule ? " (WEB)" : "")}");

	Console.Write("    Maintained By:      ");

	Console.ForegroundColor = ConsoleColor.Magenta;
	Console.WriteLine(module.Maintainer);
	Console.ResetColor();

	if (module.Contributors.Length > 0)
	{
		Console.Write("    Contributions From: ");

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

var builder = WebApplication.CreateBuilder();

builder.WebHost.CaptureStartupErrors(true);

builder.WebHost.UseUrls("http://0.0.0.0:80/");

var serviceCacher = new ServiceCacher();

builder.Services.AddSingleton(serviceCacher);

var authorizationPolicies = new List<string>();

try
{
	foreach (var startup in modules)
		startup.AddLogging(builder.Logging);

	ConsoleCreator.AddSubHeading("Successfully Initialized: ", "Logging.");

	foreach (var startup in modules)
		startup.AddPreServices(builder.Services, serviceCacher, databaseBuilder);

	ConsoleCreator.AddSubHeading("Successfully Initialized: ", "Pre-Services.");

	foreach (var startup in modules)
		startup.AddServices(builder.Services, serviceCacher, settings);
	ConsoleCreator.AddSubHeading("Successfully Initialized: ", "Services.");

	foreach (var startup in modules)
		startup.ConfigureServices(builder.Configuration, builder.Services);
	ConsoleCreator.AddSubHeading("Successfully Initialized: ", "Services.");

	foreach (var startup in modules)
		if (startup is WebModule module)
		{
			var authorizationPolicy = module.AddAuthorizationPolicy();

			if (authorizationPolicy.Length > 0)
				authorizationPolicies = authorizationPolicies.Union(authorizationPolicy).ToList();
		}

	ConsoleCreator.AddSubHeading("Successfully Added: ", "Authentication Policies.");
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

foreach (var assembly in serviceCacher.Dependents)
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
	foreach (var dataContext in serviceCacher.GetInitializedClasses<DbContext>(scope.ServiceProvider))
	{
		ConsoleCreator.AddSubHeading("Adding Migrations For: ", dataContext.GetType().Name);
		await dataContext.Database.MigrateAsync();
	}
}

ConsoleCreator.AddSubHeading("Successfully Added: ", "Migrations To Databases.");

// CONFIGURE

ConsoleCreator.AddHeading("Building MASZ");

foreach (var startup in modules)
{
	startup.PostBuild(app.Services, serviceCacher);

	if (startup is WebModule module)
		module.PostWebBuild(app, settings);
}

ConsoleCreator.AddSubHeading("Successfully: ", "Post Built Application.");

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