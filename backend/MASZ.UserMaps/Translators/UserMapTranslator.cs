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
			Language.Fr => $"UserMap entre {userMap.UserA} et {userMap.UserB}.",
			Language.Es => $"UserMap entre {userMap.UserA} y {userMap.UserB}.",
			Language.Ru => $"UserMap между {userMap.UserA} и {userMap.UserB}.",
			Language.It => $"UserMap tra {userMap.UserA} e {userMap.UserB}.",
			_ => $"UserMap between {userMap.UserA} and {userMap.UserB}."
		};
	}

	public string UserMap()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehung",
			Language.At => "Benutzabeziehung",
			Language.Fr => "UserMap",
			Language.Es => "UserMap",
			Language.Ru => "UserMap",
			Language.It => "Mappa utente",
			_ => "UserMap"
		};
	}

	public string UserMaps()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehungen",
			Language.At => "Benutzabeziehungen",
			Language.Fr => "UserMaps",
			Language.Es => "UserMaps",
			Language.Ru => "UserMaps",
			Language.It => "Mappe utente",
			_ => "UserMaps"
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
			_ => "User Note ID"
		};
	}
}