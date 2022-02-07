using Bot.Abstractions;

namespace MOTDs;

public class MotdModule : Module
{
	public override string Maintainer => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster", "FlixProd" };
}