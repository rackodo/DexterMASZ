using System.Diagnostics;

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
		Console.Write(name);

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(value);

		Console.ResetColor();
	}

	public static string AskAndSet(string name, string envVar)
	{
		var option = Ask(name, envVar);

		if (GetEnvironmentalVariable(envVar) != option)
			SetEnvironmentalVariable(envVar, option);

		return option;
	}

	public static string Ask(string name, string envVar)
	{
		var option = GetEnvironmentalVariable(envVar);

		if (!string.IsNullOrEmpty(option)) return option;

		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write($"Please enter your {name}: ");
		Console.ResetColor();
		option = Console.ReadLine();

		return option;
	}

	public static T AskDefinedChoice<T>(string name, string envVar, bool setIfNotFound = true) where T : Enum
	{
		var type = typeof(T);

		while (true)
		{
			if (Enum.TryParse(type, Ask(name, envVar), true, out var enumType))
			{
				if (enumType is not null && Enum.IsDefined(type, enumType))
				{
					var enumName = Enum.GetName(type, enumType);

					if (!setIfNotFound) return (T)enumType;

					if (GetEnvironmentalVariable(envVar) != enumName)
						SetEnvironmentalVariable(envVar, enumName);

					return (T)enumType;
				}
			}

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