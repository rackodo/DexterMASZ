using MASZ.Bot.Enums;

namespace MASZ.Bot.Abstractions;

public abstract class Translator
{
	protected Translator(Language preferredLanguage = Language.En)
	{
		PreferredLanguage = preferredLanguage;
	}

	public Language PreferredLanguage { get; set; }
}