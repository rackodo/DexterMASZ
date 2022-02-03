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
			Language.At => "Automodaration",
			Language.Fr => "Automodération",
			Language.Es => "Automoderación",
			Language.Ru => "Автомобильная промышленность",
			Language.It => "Automoderazione",
			_ => "Automod"
		};
	}
}