using Bot.Abstractions;

namespace UserMaps;

public class UserMapModule : Module
{
	public override string Creator => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster" };
}