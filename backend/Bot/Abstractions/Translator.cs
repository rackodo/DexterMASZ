using Bot.Enums;

namespace Bot.Abstractions;

public abstract class Translator
{
	protected Translator(Language preferredLanguage = Language.En)
	{
		PreferredLanguage = preferredLanguage;
	}

	public Language PreferredLanguage { get; set; }
}