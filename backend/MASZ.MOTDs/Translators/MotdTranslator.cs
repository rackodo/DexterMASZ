using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.MOTDs.Translators;

public class MotdTranslator : Translator
{
	public string MessageOfTheDay()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht des Tages",
			Language.At => "Nochricht vom Tog",
			Language.Fr => "Le message du jour",
			Language.Es => "Mensaje del día",
			Language.Ru => "Послание дня",
			Language.It => "Messaggio del giorno",
			_ => "Message of the Day"
		};
	}
}