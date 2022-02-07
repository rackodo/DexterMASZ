using Bot.Dynamics;
using Bot.Enums;
using Bot.Models;
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

	public static bool WaitForUser(string objEditName, int waitTime)
	{
		var original = DateTime.Now;
		var remainingWaitTime = waitTime;
		var lastWaitTime = waitTime.ToString();

		Console.WriteLine();

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.Write($"Press any key to {objEditName} before: ");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.Write(waitTime);

		bool keyRead;
		do
		{
			keyRead = Console.KeyAvailable;
			if (!keyRead)
			{
				var newTime = DateTime.Now;
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

		Console.WriteLine();

		Console.ResetColor();

		return keyRead;
	}

	public static void AddSubHeading(string name, string value)
	{
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.Write($"{name}: ");

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(value);

		Console.ResetColor();
	}

	public static KeyValuePair<DatabaseSettings, bool> CreateDatabaseSettings(bool isEdit)
	{
		var (host, updatedHost) = AskAndSet("database server host", "MYSQL_HOST", !isEdit);
		var (port, updatedPort) = AskAndSet("database server port", "MYSQL_PORT", !isEdit);
		var (database, updatedDatabase) = AskAndSet("database name", "MYSQL_DATABASE", !isEdit);
		var (user, updatedUser) = AskAndSet("database login username", "MYSQL_USER", !isEdit);
		var (pass, updatedPass) = AskAndSet("database login password", "MYSQL_PASSWORD", !isEdit);

		KeyValuePair<ulong, bool> clientId;

		while (true)
		{
			var client = 
				AskAndSet("Discord OAuth Client ID", "DISCORD_OAUTH_CLIENT_ID", !isEdit);

			if (ulong.TryParse(client.Key, out var id))
			{
				clientId = new(id, client.Value);
				break;
			}
		}

		var hasChanged = false;

		if (updatedHost || updatedPort || updatedDatabase || updatedUser || updatedPass || clientId.Value)
			hasChanged = true;

		return new(new DatabaseSettings
		{
			ClientId = clientId.Key,
			Database = database,
			Host = host,
			Port = port,
			User = user,
			Pass = pass
		}, hasChanged);
	}

	public static AppSettings CreateAppSettings(ClientIdContainer clientIdContainer, bool isEdit)
	{
		var settings = new AppSettings
		{
			ClientId = clientIdContainer.ClientId,
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

	public static KeyValuePair<string, bool> AskAndSet(string name, string envVar, bool useEnvVars)
	{
		var option = Ask(name, envVar, useEnvVars);

		if (GetEnvironmentalVariable(envVar) != option)
		{
			SetEnvironmentalVariable(envVar, option);
			return new(option, true);
		}
		else
			return new(option, false);
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