using Bot.Abstractions;

namespace UserNotes;

public class UserNoteModule : Module
{
	public override string Creator => "Zaanposni";

	public override string[] Contributors { get; } = { "Ferox" };

	public override string[] Translators { get; } = { "Bricksmaster" };
}