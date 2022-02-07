using Bot.Abstractions;

namespace Bot.Translators;

public class BotTranslator : Translator
{
	public string Action()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Aktion",
			Enums.Language.At => "Aktio",
			Enums.Language.Fr => "action",
			Enums.Language.Es => "Acción",
			Enums.Language.Ru => "Действие",
			Enums.Language.It => "Azione",
			_ => "Action"
		};
	}

	public string GuildId()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Gilden-ID",
			Enums.Language.At => "Güdn-ID",
			Enums.Language.Fr => "Veuillez spécifier un identifiant de guilde valide.",
			Enums.Language.Es => "Guild ID",
			Enums.Language.Ru => "Идентификатор гильдии.",
			Enums.Language.It => "ID Gilda",
			_ => "Guild ID"
		};
	}

	public string Author()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Autor",
			Enums.Language.At => "Autoa",
			Enums.Language.Fr => "Auteur",
			Enums.Language.Es => "Autor",
			Enums.Language.Ru => "Автор",
			Enums.Language.It => "Autore",
			_ => "Author"
		};
	}

	public string Id()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "ID",
			Enums.Language.At => "ID",
			Enums.Language.Fr => "identifiant",
			Enums.Language.Es => "IDENTIFICACIÓN",
			Enums.Language.Ru => "Я БЫ",
			Enums.Language.It => "ID",
			_ => "ID"
		};
	}

	public string User()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Nutzer",
			Enums.Language.At => "Nutza",
			Enums.Language.Fr => "Utilisateur",
			Enums.Language.Es => "Usuario",
			Enums.Language.Ru => "Пользователь",
			Enums.Language.It => "Utente",
			_ => "User"
		};
	}

	public string UserId()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "NutzerId",
			Enums.Language.At => "NutzaId",
			Enums.Language.Fr => "Identifiant d'utilisateur",
			Enums.Language.Es => "User ID",
			Enums.Language.Ru => "ID пользователя",
			Enums.Language.It => "ID utente",
			_ => "User ID"
		};
	}

	public string Channel()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Kanal",
			Enums.Language.At => "Kanoi",
			Enums.Language.Fr => "Canaliser",
			Enums.Language.Es => "Canal",
			Enums.Language.Ru => "Канал",
			Enums.Language.It => "Canale",
			_ => "Channel"
		};
	}

	public string ChannelId()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "KanalId",
			Enums.Language.At => "KanalId",
			Enums.Language.Fr => "Identifiant de la chaine",
			Enums.Language.Es => "Canal ID",
			Enums.Language.Ru => "ChannelId",
			Enums.Language.It => "Canale ID",
			_ => "ChannelId"
		};
	}

	public string NotFound()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Nicht gefunden.",
			Enums.Language.At => "Ned gfundn.",
			Enums.Language.Fr => "Pas trouvé.",
			Enums.Language.Es => "Extraviado.",
			Enums.Language.Ru => "Не найден.",
			Enums.Language.It => "Non trovato.",
			_ => "Not found."
		};
	}

	public string MessageContent()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Nachrichteninhalt",
			Enums.Language.At => "Nochrichtninhoit",
			Enums.Language.Fr => "Contenu du message",
			Enums.Language.Es => "Contenido del mensaje",
			Enums.Language.Ru => "Содержание сообщения",
			Enums.Language.It => "Contenuto del messaggio",
			_ => "Message content"
		};
	}

	public string Attachments()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Anhänge",
			Enums.Language.At => "Ohäng",
			Enums.Language.Fr => "Pièces jointes",
			Enums.Language.Es => "Archivos adjuntos",
			Enums.Language.Ru => "Вложения",
			Enums.Language.It => "Allegati",
			_ => "Attachments"
		};
	}

	public string Attachment()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Anhang",
			Enums.Language.At => "Ohang",
			Enums.Language.Fr => "Attachement",
			Enums.Language.Es => "Adjunto",
			Enums.Language.Ru => "Вложение",
			Enums.Language.It => "allegato",
			_ => "Attachment"
		};
	}

	public string AndXMore(int count)
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => $"und {count} weitere...",
			Enums.Language.At => $"und {count} weitare...",
			Enums.Language.Fr => $"et {count} plus...",
			Enums.Language.Es => $"y {count} más ...",
			Enums.Language.Ru => $"и еще {count} ...",
			Enums.Language.It => $"e {count} altro...",
			_ => $"and {count} more..."
		};
	}

	public string Until()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "bis",
			Enums.Language.At => "bis",
			Enums.Language.Fr => "jusqu'à",
			Enums.Language.Es => "Hasta que",
			Enums.Language.Ru => "до",
			Enums.Language.It => "fino a",
			_ => "until"
		};
	}

	public string Description()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Beschreibung",
			Enums.Language.At => "Beschreibung",
			Enums.Language.Fr => "La description",
			Enums.Language.Es => "Descripción",
			Enums.Language.Ru => "Описание",
			Enums.Language.It => "Descrizione",
			_ => "Description"
		};
	}

	public string Labels()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Labels",
			Enums.Language.At => "Labl",
			Enums.Language.Fr => "Étiquettes",
			Enums.Language.Es => "Etiquetas",
			Enums.Language.Ru => "Этикетки",
			Enums.Language.It => "etichette",
			_ => "Labels"
		};
	}

	public string Filename()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Dateiname",
			Enums.Language.At => "Dateinom",
			Enums.Language.Fr => "Nom de fichier",
			Enums.Language.Es => "Nombre del archivo",
			Enums.Language.Ru => "Имя файла",
			Enums.Language.It => "Nome del file",
			_ => "Filename"
		};
	}

	public string Message()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Nachricht",
			Enums.Language.At => "Nochricht",
			Enums.Language.Fr => "Un message",
			Enums.Language.Es => "Mensaje",
			Enums.Language.Ru => "Сообщение",
			Enums.Language.It => "Messaggio",
			_ => "Message"
		};
	}

	public string Type()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Typ",
			Enums.Language.At => "Typ",
			Enums.Language.Fr => "Taper",
			Enums.Language.Es => "Escribe",
			Enums.Language.Ru => "Тип",
			Enums.Language.It => "Tipo",
			_ => "Type"
		};
	}

	public string Joined()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Beigetreten",
			Enums.Language.At => "Beigetretn",
			Enums.Language.Fr => "Inscrit",
			Enums.Language.Es => "Unido",
			Enums.Language.Ru => "Присоединился",
			Enums.Language.It => "Partecipato",
			_ => "Joined"
		};
	}

	public string Registered()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Registriert",
			Enums.Language.At => "Registriat",
			Enums.Language.Fr => "Inscrit",
			Enums.Language.Es => "Registrado",
			Enums.Language.Ru => "Зарегистрировано",
			Enums.Language.It => "Registrato",
			_ => "Registered"
		};
	}

	public string OnlyTextChannel()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Nur Textkanäle sind erlaubt.",
			Enums.Language.At => "Nua Textkanö san guat.",
			Enums.Language.Fr => "Seuls les canaux de texte sont autorisés.",
			Enums.Language.Es => "Solo se permiten canales de texto.",
			Enums.Language.Ru => "Разрешены только текстовые каналы.",
			Enums.Language.It => "Sono consentiti solo canali di testo.",
			_ => "Only text channels are allowed."
		};
	}

	public string CannotViewOrDeleteInChannel()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Ich darf keine Nachrichten in diesem Kanal sehen oder löschen!",
			Enums.Language.At => "I derf kane Nochrichtn in dem Kanoi sehn oda leschn!",
			Enums.Language.Fr => "Je ne suis pas autorisé à afficher ou supprimer les messages de cette chaîne !",
			Enums.Language.Es => "¡No puedo ver ni borrar mensajes en este canal!",
			Enums.Language.Ru => "Мне не разрешено просматривать или удалять сообщения на этом канале!",
			Enums.Language.It => "Non sono autorizzato a visualizzare o eliminare i messaggi in questo canale!",
			_ => "I'm not allowed to view or delete messages in this channel!"
		};
	}

	public string CannotFindChannel()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Kanal konnte nicht gefunden werden.",
			Enums.Language.At => "Kanoi konnt ned gfundn wan.",
			Enums.Language.Fr => "Impossible de trouver la chaîne.",
			Enums.Language.Es => "No se puede encontrar el canal.",
			Enums.Language.Ru => "Не могу найти канал.",
			Enums.Language.It => "Impossibile trovare il canale.",
			_ => "Cannot find channel."
		};
	}

	public string NoWebhookConfigured()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Dieser Server hat keinen internen Webhook für Benachrichtigungen konfiguriert.",
			Enums.Language.At => "Da Serva hot kan internan Webhook fia Benochrichtigungen konfiguriat.",
			Enums.Language.Fr => "Cette guilde n'a pas configuré de webhook pour les notifications internes.",
			Enums.Language.Es => "Este gremio no tiene configurado ningún webhook para notificaciones internas.",
			Enums.Language.Ru => "У этой гильдии нет настроенного веб-перехватчика для внутренних уведомлений.",
			Enums.Language.It => "Questa gilda non ha webhook per le notifiche interne configurate.",
			_ => "This guild has no webhook for internal notifications configured."
		};
	}

	public string SomethingWentWrong()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Etwas ist schief gelaufen.",
			Enums.Language.At => "Etwos hot ned funktioniat.",
			Enums.Language.Fr => "Quelque chose s'est mal passé.",
			Enums.Language.Es => "Algo salió mal.",
			Enums.Language.Ru => "Что-то пошло не так.",
			Enums.Language.It => "Qualcosa è andato storto.",
			_ => "Something went wrong."
		};
	}

	public string Code()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Code",
			Enums.Language.At => "Code",
			Enums.Language.Fr => "Code",
			Enums.Language.Es => "Código",
			Enums.Language.Ru => "Код",
			Enums.Language.It => "Codice",
			_ => "Code"
		};
	}

	public string Language()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Sprache",
			Enums.Language.At => "Sproch",
			Enums.Language.Fr => "Langue",
			Enums.Language.Es => "Idioma",
			Enums.Language.Ru => "Язык",
			Enums.Language.It => "Lingua",
			_ => "Language"
		};
	}

	public string Timestamps()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Zeitstempel",
			Enums.Language.At => "Zeitstempl",
			Enums.Language.Fr => "Horodatage",
			Enums.Language.Es => "Marcas de tiempo",
			Enums.Language.Ru => "Отметки времени",
			Enums.Language.It => "Timestamp",
			_ => "Timestamps"
		};
	}

	public string Support()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Support",
			Enums.Language.At => "Supoat",
			Enums.Language.Fr => "Soutien",
			Enums.Language.Es => "Apoyo",
			Enums.Language.Ru => "Служба поддержки",
			Enums.Language.It => "Supporto",
			_ => "Support"
		};
	}

	public string Features()
	{
		return PreferredLanguage switch
		{
			Enums.Language.De => "Features",
			Enums.Language.At => "Features",
			Enums.Language.Fr => "Caractéristiques",
			Enums.Language.Es => "Características",
			Enums.Language.Ru => "Функции",
			Enums.Language.It => "Caratteristiche",
			_ => "Features"
		};
	}
}