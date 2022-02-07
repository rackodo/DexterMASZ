using Bot.Abstractions;

namespace AutoMods;

public class AutoModModule : Module
{
	public override string Creator => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster" };
}