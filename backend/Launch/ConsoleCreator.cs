using MASZ.Bot.Enums;
using MASZ.Bot.Models;
using System.Diagnostics;
using System.Reflection;

namespace Launch;

public static class ConsoleCreator
{
	public const int HeadingPadding = 36;

	public static void AddHeading(string name)
	{
		name = $" {name} ";
		var padLeft = (HeadingPadding - name.Length) / 2 + name.Length;
		Console.ResetColor();
		Console.WriteLine($"\n{name.PadLeft(padLeft, '=').PadRight(HeadingPadding, '=')}\n");
	}

	public static void AddSubHeading(string name, string value)
	{
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.Write($"{name}: ");

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(value);

		Console.ResetColor();
	}

	public static AppSettings CreateAppSettings(ulong clientID, bool isEdit)
	{
		var settings = new AppSettings
		{
			ClientId = clientID,
			ClientSecret = Ask("Discord OAuth Client Secret", "DISCORD_OAUTH_CLIENT_SECRET", !isEdit),

			DiscordBotToken = Ask("Discord bot token", "DISCORD_BOT_TOKEN", !isEdit),
			AuditLogWebhookUrl = Ask(
				"audit log webhook url, recommended to be in a private channel for site admins " +
				"as it may log sensitive information, or leave empty to disable", "AUDIT_LOG_WEBHOOK_URL", !isEdit, true),

			Lang = Enum.GetName(
				AskDefinedChoice<Language>("default language", "DEFAULT_LANGUAGE", !isEdit)),

			PublicFileMode = AskDefinedChoice<Booleans>("stance on whether files should be public",
				"ENABLE_PUBLIC_FILES", !isEdit) == Booleans.True,
			CorsEnabled = AskDefinedChoice<Booleans>(
				"stance on whether this is in a development environment",
				"ENABLE_CORS", !isEdit) == Booleans.True,
			DemoModeEnabled = AskDefinedChoice<Booleans>(
				"stance on whether this is being used as a demonstration",
				"ENABLE_DEMO_MODE", !isEdit) == Booleans.True
		};

		var admins =
			Ask("site administrator ids, recommended as just one, but can be split by ','",
				"DISCORD_SITE_ADMINS", !isEdit);

		if (!string.IsNullOrEmpty(admins))
			settings.SiteAdmins = admins.Split(',').Select(ulong.Parse).ToArray();

		var directoryPath = Ask("directory for files to be saved (leave empty for current)",
			"ABSOLUTE_PATH_TO_FILE_UPLOAD", !isEdit, true);

		if (string.IsNullOrEmpty(directoryPath))
			directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		settings.AbsolutePathToFileUpload = directoryPath;

		switch (AskDefinedChoice<DeploymentType>("stance on whether this is being deployed on " +
			"a domain or locally, as a test version", "DEPLOY_MODE", !isEdit))
		{
			case DeploymentType.Domain:
				settings.ServiceHostName = Ask("service name", "META_SERVICE_NAME", !isEdit);
				settings.ServiceDomain = Ask("(sub)domain", "META_SERVICE_DOMAIN", !isEdit);
				settings.ServiceBaseUrl = $"https://{settings.ServiceDomain}";
				AddSubHeading("Alert", "Be sure to redirect your reverse proxy correctly");
				AddSubHeading("The docker container will be listening on local port", "5565");
				break;
			case DeploymentType.Local:
				settings.ServiceHostName = Ask("service name", "META_SERVICE_NAME", !isEdit);
				settings.ServiceDomain = "127.0.0.1:5565";
				settings.ServiceBaseUrl = $"http://{settings.ServiceDomain}";
				break;
			default:
				throw new NotImplementedException();
		}

		return settings;
	}

	public static string AskAndSet(string name, string envVar)
	{
		var option = Ask(name, envVar, true);

		if (GetEnvironmentalVariable(envVar) != option)
			SetEnvironmentalVariable(envVar, option);

		return option;
	}

	public static string Ask(string name, string envVar, bool useEnvVars, bool ignoreIfEmpty = false)
	{
		string option;

		if (useEnvVars)
		{
			option = GetEnvironmentalVariable(envVar);

			if (!string.IsNullOrEmpty(option))
				return option;
		}

		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write($"Please enter your {name}: ");
		Console.ResetColor();
		option = Console.ReadLine();

		if (option == string.Empty && !ignoreIfEmpty)
			return Ask(name, envVar, useEnvVars, ignoreIfEmpty);
		else
			return option;
	}

	public static T AskDefinedChoice<T>(string name, string envVar, bool useEnvVars) where T : Enum
	{
		var type = typeof(T);

		while (true)
		{
			if (Enum.TryParse(type, Ask(name, envVar, useEnvVars, true), true, out var enumType))
				if (enumType is not null && Enum.IsDefined(type, enumType))
					return (T)enumType;

			if (useEnvVars)
				if (!string.IsNullOrEmpty(GetEnvironmentalVariable(envVar)))
					SetEnvironmentalVariable(envVar, "");

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Please enter a valid {name}!");

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Options: {string.Join(", ", (T[])Enum.GetValues(type))}");

			Console.ResetColor();
		}
	}

	public static string GetEnvironmentalVariable(string envVar)
	{
		return Environment.OSVersion.Platform switch
		{
			PlatformID.Win32NT => Environment.GetEnvironmentVariable(envVar, EnvironmentVariableTarget.User),
			PlatformID.Unix => Environment.GetEnvironmentVariable(envVar),
			_ => throw new NotImplementedException()
		};
	}

	public static void SetEnvironmentalVariable(string envVar, string option)
	{
		switch (Environment.OSVersion.Platform)
		{
			case PlatformID.Win32NT:
				Environment.SetEnvironmentVariable(envVar, option, EnvironmentVariableTarget.User);
				break;
			case PlatformID.Unix:
				Process.Start("/bin/bash", "-c export " + envVar + "=" + option);
				break;
			default:
				throw new NotImplementedException();
		}
	}
}