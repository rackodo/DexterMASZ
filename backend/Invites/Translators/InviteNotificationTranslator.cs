using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Discord;

namespace Invites.Translators;

public class InviteNotificationTranslator : Translator
{
	public string Registered()
	{
		return PreferredLanguage switch
		{
			Language.De => "Registriert",
			Language.Fr => "Enregistré",
			Language.Es => "Registrado",
			Language.Ru => "зарегистрированный",
			Language.It => "Registrato",
			_ => "Registered"
		};
	}

	public string Invite()
	{
		return PreferredLanguage switch
		{
			Language.De => "Invite",
			Language.Fr => "L'invitation",
			Language.Es => "La Invitación",
			Language.Ru => "Приглашать",
			Language.It => "L'invito",
			_ => "Invite"
		};
	}

	public string Created()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erstellt",
			Language.Fr => "Créé",
			Language.Es => "Creada",
			Language.Ru => "Созданный",
			Language.It => "Creata",
			_ => "Created"
		};
	}

	public string By()
	{
		return PreferredLanguage switch
		{
			Language.De => "Durch",
			Language.Fr => "Par",
			Language.Es => "Por",
			Language.Ru => "К",
			Language.It => "Di",
			_ => "By"
		};
	}
}