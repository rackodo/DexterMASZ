using Bot.Abstractions;
using Bot.Enums;

namespace MOTDs.Translators;

public class MotdTranslator : Translator
{
	public string MessageOfTheDay()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht des Tages",
			Language.Fr => "Le message du jour",
			Language.Es => "Mensaje del día",
			Language.Ru => "Послание дня",
			Language.It => "Messaggio del giorno",
			_ => "Message of the Day"
		};
	}
}