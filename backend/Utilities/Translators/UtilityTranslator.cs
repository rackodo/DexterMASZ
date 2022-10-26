using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace Utilities.Translators;

public class UtilityTranslator : Translator
{
	public string GuildAvatarUrl()
	{
		return PreferredLanguage switch
		{
			Language.De => "Holen Sie sich die Guild Avatar-URL",
			Language.Fr => "Obtenir l'URL de l'avatar guild",
			Language.Es => "Obtener URL de avatar guild",
			Language.Ru => "Получить URL-адрес аватара guild",
			Language.It => "Ottieni l'URL dell'avatar guild",
			_ => "Get Guild Avatar URL"
		};
	}
	public string BannerUrl()
	{
		return PreferredLanguage switch
		{
			Language.De => "Holen Sie sich die Banner-URL",
			Language.Fr => "Obtenir l'URL de l'banner",
			Language.Es => "Obtener URL de banner",
			Language.Ru => "Получить URL-адрес баннера",
			Language.It => "Ottieni URL banner",
			_ => "Get Bannel URL"
		};
	}
	public string AvatarUrl()
	{
		return PreferredLanguage switch
		{
			Language.De => "Holen Sie sich die Avatar-URL",
			Language.Fr => "Obtenir l'URL de l'avatar",
			Language.Es => "Obtener URL de avatar",
			Language.Ru => "Получить URL-адрес аватара",
			Language.It => "Ottieni l'URL dell'avatar",
			_ => "Get Avatar URL"
		};
	}
	public string Status()
	{
		return PreferredLanguage switch
		{
			Language.De => "Status",
			Language.Fr => "Statut",
			Language.Es => "Estado",
			Language.Ru => "Статус",
			Language.It => "Stato",
			_ => "Status",
		};
	}
	public string UserProfile()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzerprofil für",
			Language.Fr => "Profil utilisateur pour",
			Language.Es => "Perfil de usuario para",
			Language.Ru => "Профиль пользователя для",
			Language.It => "Profilo utente per",
			_ => "User Profile For",
		};
	}
	public string Bot()
	{
		return PreferredLanguage switch
		{
			Language.De => "Bot",
			Language.Fr => "Bot",
			Language.Es => "Bot",
			Language.Ru => "Бот",
			Language.It => "Bot",
			_ => "Bot",
		};
	}
	public string Database()
	{
		return PreferredLanguage switch
		{
			Language.De => "Datenbank",
			Language.Fr => "Base de données",
			Language.Es => "Base de datos",
			Language.Ru => "База данных",
			Language.It => "Database",
			_ => "Database",
		};
	}
	public string InternalCache()
	{
		return PreferredLanguage switch
		{
			Language.De => "Interner Cache",
			Language.Fr => "Cache interne",
			Language.Es => "Cache interno",
			Language.Ru => "Внутренний кеш",
			Language.It => "Cache interno",
			_ => "Internal Cache",
		};
	}
	public string CurrentlyLoggedIn()
	{
		return PreferredLanguage switch
		{
			Language.De => "Momentan angemeldete Benutzer",
			Language.Fr => "Utilisateurs actuellement connectés",
			Language.Es => "Usuarios actualmente conectados",
			Language.Ru => "Пользователи, в настоящее время в системе",
			Language.It => "Utenti attualmente collegati",
			_ => "Currently logged in users",
		};
	}
	public string LastDisconnectAt(string time)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Letzter Abmeldungszeitpunkt: {time}.",
			Language.Fr => $"Dernière déconnexion à {time}.",
			Language.Es => $"Última desconexión en {time}.",
			Language.Ru => $"Последнее отключение: {time}.",
			Language.It => $"Ultima disconnessione a {time}.",
			_ => $"Experienced last disconnect at {time}.",
		};
	}
	public string DeletedMessages(int count, IMentionable channel)
	{
		return PreferredLanguage switch
		{
			Language.De => $"{count} Nachrichten in {channel.Mention} gelöscht.",
			Language.Fr => $"{count} messages supprimés dans {channel.Mention}.",
			Language.Es => $"Se eliminaron {count} mensajes en {channel.Mention}.",
			Language.Ru => $"Удалено {count} сообщений в {channel.Mention}.",
			Language.It => $"Eliminati {count} messaggi in {channel.Mention}.",
			_ => $"Deleted {count} messages in {channel.Mention}."
		};
	}
}