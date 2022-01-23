using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.MOTDs.Translators;

public class MotdNotificationTranslator : Translator
{
	public string NotificationMotdInternalCreate(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Neue MotD wurde von {actor.Mention} erstellt.",
			Language.At => $"A neiche MotD wuad vo {actor.Mention} erstöt. ",
			Language.Fr => $"Le nouveau MotD a été créé par {actor.Mention}.",
			Language.Es => $"El nuevo MotD ha sido creado por {actor.Mention}.",
			Language.Ru => $"Новый MotD был создан {actor.Mention}.",
			Language.It => $"Il nuovo MotD è stato creato da {actor.Mention}.",
			_ => $"New MotD has been created by {actor.Mention}."
		};
	}

	public string NotificationMotdInternalEdited(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"MotD wurde von {actor.Mention} bearbeitet.",
			Language.At => $"MotD is vo {actor.Mention} beorbeit woan.",
			Language.Fr => $"MotD a été édité par {actor.Mention}.",
			Language.Es => $"MotD ha sido editado por {actor.Mention}.",
			Language.Ru => $"MotD редактировал {actor.Mention}.",
			Language.It => $"MotD è stato modificato da {actor.Mention}.",
			_ => $"MotD has been edited by {actor.Mention}."
		};
	}

	public string NotificationMotdShow()
	{
		return PreferredLanguage switch
		{
			Language.De => "Anzeigen",
			Language.At => "Ozeign",
			Language.Fr => "Montrer",
			Language.Es => "Show",
			Language.Ru => "Показывать",
			Language.It => "Spettacolo",
			_ => "Show"
		};
	}
}