using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Utilities.Translators;

public class UtilityTranslator : Translator
{
	public string AvatarUrl()
	{
		return PreferredLanguage switch
		{
			Language.De => "Avatar URL",
			Language.At => "Avadar URL",
			Language.Fr => "l'URL d'avatar",
			Language.Es => "URL de avatar",
			Language.Ru => "URL аватара",
			Language.It => "l'URL dell'avatar",
			_ => "Avatar URL"
		};
	}

	public string Status()
	{
		return PreferredLanguage switch
		{
			Language.De => "Status",
			Language.At => "Status",
			Language.Fr => "Statut",
			Language.Es => "Estado",
			Language.Ru => "Статус",
			Language.It => "Stato",
			_ => "Status",
		};
	}
	public string Bot()
	{
		return PreferredLanguage switch
		{
			Language.De => "Bot",
			Language.At => "Bot",
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
			Language.At => "Datenbank",
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
			Language.At => "Interner Cache",
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
			Language.At => "Momentan angemeldete Benutzer",
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
			Language.At => $"Letzter Abmeldungszeitpunkt: {time}.",
			Language.Fr => $"Dernière déconnexion à {time}.",
			Language.Es => $"Última desconexión en {time}.",
			Language.Ru => $"Последнее отключение: {time}.",
			Language.It => $"Ultima disconnessione a {time}.",
			_ => $"Experienced last disconnect at {time}.",
		};
	}
	
	public string Invite()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Du musst deine eigene Instanz von MASZ auf deinem Server oder PC hosten.\nSchau dir https://github.com/zaanposni/discord-masz#hosting an",
			Language.At =>
				"Du muast dei eignane Inszanz vo MASZ auf deim Serva oda PC hosn.\nSchau da https://github.com/zaanposni/discord-masz#hosting o",
			Language.Fr =>
				"Vous devrez héberger votre propre instance de MASZ sur votre serveur ou votre PC.\nCommander https://github.com/zaanposni/discord-masz#hosting",
			Language.Es =>
				"Tendrá que alojar su propia instancia de MASZ en su servidor o PC.\nPagar https://github.com/zaanposni/discord-masz#hosting",
			Language.Ru =>
				"Вам нужно будет разместить свой собственный экземпляр MASZ на вашем сервере или компьютере.\nОформить заказ https://github.com/zaanposni/discord-masz#hosting",
			Language.It =>
				"Dovrai ospitare la tua istanza di MASZ sul tuo server o PC.\nAcquista https://github.com/zaanposni/discord-masz#hosting",
			_ =>
				"You will have to host your own instance of MASZ on your server or pc.\nCheck out https://github.com/zaanposni/discord-masz#hosting"
		};
	}

	public string DeletedMessages(int count, IMentionable channel)
	{
		return PreferredLanguage switch
		{
			Language.De => $"{count} Nachrichten in {channel.Mention} gelöscht.",
			Language.At => $"{count} Nochrichtn in {channel.Mention} glescht.",
			Language.Fr => $"{count} messages supprimés dans {channel.Mention}.",
			Language.Es => $"Se eliminaron {count} mensajes en {channel.Mention}.",
			Language.Ru => $"Удалено {count} сообщений в {channel.Mention}.",
			Language.It => $"Eliminati {count} messaggi in {channel.Mention}.",
			_ => $"Deleted {count} messages in {channel.Mention}."
		};
	}
}