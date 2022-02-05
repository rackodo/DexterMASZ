using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.AutoMods.Translators;

public class AutoModTranslator : Translator
{
	public string AutoMod()
	{
		return PreferredLanguage switch
		{
			Language.De => "Automod",
			Language.At => "Automod",
			Language.Fr => "Automod",
			Language.Es => "Automod",
			Language.Ru => "Автоmод",
			Language.It => "Automod",
			_ => "Automod"
		};
	}
}