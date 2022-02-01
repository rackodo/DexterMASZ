using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.UserMaps.Models;

namespace MASZ.UserMaps.Translators;

public class UserMapTranslator : Translator
{
	public string UserMapBetween(UserMap userMap)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Benutzerbeziehung zwischen {userMap.UserA} und {userMap.UserB}.",
			Language.At => $"Benutzabeziehung zwischa {userMap.UserA} und {userMap.UserB}.",
			Language.Fr => $"Usermap entre {userMap.UserA} et {userMap.UserB}.",
			Language.Es => $"Usermap entre {userMap.UserA} y {userMap.UserB}.",
			Language.Ru => $"Usermap между {userMap.UserA} и {userMap.UserB}.",
			Language.It => $"Usermap tra {userMap.UserA} e {userMap.UserB}.",
			_ => $"Usermap between {userMap.UserA} and {userMap.UserB}."
		};
	}

	public string UserMap()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehung",
			Language.At => "Benutzabeziehung",
			Language.Fr => "Usermap",
			Language.Es => "Usermap",
			Language.Ru => "Usermap",
			Language.It => "Mappa utente",
			_ => "Usermap"
		};
	}

	public string UserMaps()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehungen",
			Language.At => "Benutzabeziehungen",
			Language.Fr => "Usermaps",
			Language.Es => "Usermaps",
			Language.Ru => "Usermaps",
			Language.It => "Mappe utente",
			_ => "Usermaps"
		};
	}

	public string UserMapId()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerkarten-ID",
			Language.At => "NutzaMapID",
			Language.Fr => "Identifiant de la carte utilisateur",
			Language.Es => "ID de mapa de usuario",
			Language.Ru => "идентификатор пользовательской карты",
			Language.It => "ID mappa utente",
			_ => "Usermap ID"
		};
	}
}