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
			Language.Fr => $"User map entre {userMap.UserA} et {userMap.UserB}.",
			Language.Es => $"User map entre {userMap.UserA} y {userMap.UserB}.",
			Language.Ru => $"User map между {userMap.UserA} и {userMap.UserB}.",
			Language.It => $"User map tra {userMap.UserA} e {userMap.UserB}.",
			_ => $"User map between {userMap.UserA} and {userMap.UserB}."
		};
	}

	public string UserMap()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehung",
			Language.Fr => "User Map",
			Language.Es => "User Map",
			Language.Ru => "User Map",
			Language.It => "Mappa Utente",
			_ => "User Map"
		};
	}

	public string UserMaps()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerbeziehungen",
			Language.Fr => "User Maps",
			Language.Es => "User Maps",
			Language.Ru => "User Maps",
			Language.It => "Mappe Utente",
			_ => "User Maps"
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
			_ => "User Map ID"
		};
	}
}
