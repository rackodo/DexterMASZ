using MASZ.Bot.Abstractions;

namespace MASZ.UserNotes;

public class UserNoteModule : Module
{
	public override string Maintainer => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster", "FlixProd" };
}