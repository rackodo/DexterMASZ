using Bot.Abstractions;
using Bot.Enums;
using UserMaps.Models;

namespace UserMaps.Translators;

public class UserMapTranslator : Translator
{
	public string UserMapBetween(UserMap userMap)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Benutzerbeziehung zwischen {userMap.UserA} und {userMap.UserB}.",
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
			Language.Fr => "UserMap",
			Language.Es => "UserMap",
			Language.Ru => "UserMap",
			Language.It => "Mappa Utente",
			_ => "UserMap"
		};
	}

	public string UserMaps()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehungen",
			Language.Fr => "UserMaps",
			Language.Es => "UserMaps",
			Language.Ru => "UserMaps",
			Language.It => "Mappe Utente",
			_ => "UserMaps"
		};
	}

	public string UserMapId()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerkarten-ID",
			Language.Fr => "Identifiant de la Carte Utilisateur",
			Language.Es => "ID de Mapa de Usuario",
			Language.Ru => "идентификатор пользовательской карты",
			Language.It => "ID Mappa Utente",
			_ => "UserMap ID"
		};
	}
}