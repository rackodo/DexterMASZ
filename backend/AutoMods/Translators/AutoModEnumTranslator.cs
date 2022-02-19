using AutoMods.Enums;
using Bot.Abstractions;
using Bot.Enums;

namespace AutoMods.Translators;

public class AutoModEnumTranslator : Translator
{
	public string Enum(AutoModAction enumValue)
	{
		return enumValue switch
		{
			AutoModAction.None => PreferredLanguage switch
			{
				Language.De => "Keine Aktion",
				Language.Fr => "Pas d'action",
				Language.Es => "Ninguna acción",
				Language.Ru => "Бездействие",
				Language.It => "Nessuna azione",
				_ => "No action"
			},
			AutoModAction.ContentDeleted => PreferredLanguage switch
			{
				Language.De => "Nachricht gelöscht",
				Language.Fr => "Contenu supprimé",
				Language.Es => "Contenido eliminado",
				Language.Ru => "Контент удален",
				Language.It => "Contenuto eliminato",
				_ => "Content deleted"
			},
			AutoModAction.CaseCreated => PreferredLanguage switch
			{
				Language.De => "Vorfall erstellt",
				Language.Fr => "Cas créé",
				Language.Es => "Caso creado",
				Language.Ru => "Дело создано",
				Language.It => "Caso creato",
				_ => "Case created"
			},
			AutoModAction.ContentDeletedAndCaseCreated => PreferredLanguage switch
			{
				Language.De => "Nachricht gelöscht und Vorfall erstellt",
				Language.Fr => "Contenu supprimé et dossier créé",
				Language.Es => "Contenido eliminado y caso creado",
				Language.Ru => "Контент удален, а дело создано",
				Language.It => "Contenuto eliminato e caso creato",
				_ => "Content deleted and case created"
			},
			_ => "Unknown"
		};
	}

	public string Enum(AutoModType enumValue)
	{
		return enumValue switch
		{
			AutoModType.InvitePosted => PreferredLanguage switch
			{
				Language.De => "Einladung gesendet",
				Language.Fr => "Invitation publiée",
				Language.Es => "Invitación publicada",
				Language.Ru => "Приглашение опубликовано",
				Language.It => "Invito pubblicato",
				_ => "Invite posted"
			},
			AutoModType.TooManyEmotes => PreferredLanguage switch
			{
				Language.De => "Zu viele Emojis verwendet",
				Language.Fr => "Trop d'émoticônes utilisées",
				Language.Es => "Demasiados emotes usados",
				Language.Ru => "Использовано слишком много эмоций",
				Language.It => "Troppe emoticon usate",
				_ => "Too many emotes used"
			},
			AutoModType.TooManyMentions => PreferredLanguage switch
			{
				Language.De => "Zu viele Benutzer erwähnt",
				Language.Fr => "Trop d'utilisateurs mentionnés",
				Language.Es => "Demasiados usuarios mencionados",
				Language.Ru => "Упомянуто слишком много пользователей",
				Language.It => "Troppi utenti citati",
				_ => "Too many users mentioned"
			},
			AutoModType.TooManyAttachments => PreferredLanguage switch
			{
				Language.De => "Zu viele Anhänge verwendet",
				Language.Fr => "Trop de pièces jointes utilisées",
				Language.Es => "Se han utilizado demasiados archivos adjuntos",
				Language.Ru => "Использовано слишком много вложений",
				Language.It => "Troppi allegati utilizzati",
				_ => "Too many attachments used"
			},
			AutoModType.TooManyEmbeds => PreferredLanguage switch
			{
				Language.De => "Zu viele Einbettungen verwendet",
				Language.Fr => "Trop d'intégrations utilisées",
				Language.Es => "Se han utilizado demasiados elementos incrustados",
				Language.Ru => "Использовано слишком много закладных",
				Language.It => "Troppi incorporamenti utilizzati",
				_ => "Too many embeds used"
			},
			AutoModType.TooManyAutomods => PreferredLanguage switch
			{
				Language.De => "Zu viele automatische punishmentsen",
				Language.Fr => "Trop de modérations automatiques",
				Language.Es => "Demasiadas moderaciones automáticas",
				Language.Ru => "Слишком много автоматических модераций",
				Language.It => "Troppe moderazioni automatiche",
				_ => "Too many auto-punishments"
			},
			AutoModType.CustomWordFilter => PreferredLanguage switch
			{
				Language.De => "Benutzerdefinierter Wortfilter ausgelöst",
				Language.Fr => "Filtre de mots personnalisé déclenché",
				Language.Es => "Filtro de palabras personalizado activado",
				Language.Ru => "Пользовательский фильтр слов активирован",
				Language.It => "Filtro parole personalizzato attivato",
				_ => "Custom wordfilter triggered"
			},
			AutoModType.TooManyMessages => PreferredLanguage switch
			{
				Language.De => "Zu viele Nachrichten",
				Language.Fr => "Trop de messages",
				Language.Es => "Demasiados mensajes",
				Language.Ru => "Слишком много сообщений",
				Language.It => "Troppi messaggi",
				_ => "Too many messages"
			},
			AutoModType.TooManyDuplicatedCharacters => PreferredLanguage switch
			{
				Language.De => "Zu viele wiederholende Buchstaben verwendet",
				Language.Fr => "Trop de caractères dupliqués utilisés",
				Language.Es => "Se han utilizado demasiados caracteres duplicados",
				Language.Ru => "Использовано слишком много повторяющихся символов",
				Language.It => "Troppi caratteri duplicati utilizzati",
				_ => "Too many duplicated characters used"
			},
			AutoModType.TooManyLinks => PreferredLanguage switch
			{
				Language.De => "Zu viele Links verwendet",
				Language.Fr => "Trop de liens utilisés",
				Language.Es => "Se han utilizado demasiados enlaces",
				Language.Ru => "Использовано слишком много ссылок",
				Language.It => "Troppi link utilizzati",
				_ => "Too many links used"
			},
			_ => "Unknown"
		};
	}

	public string Enum(AutoModChannelNotificationBehavior enumValue)
	{
		return enumValue switch
		{
			AutoModChannelNotificationBehavior.SendNotification => PreferredLanguage switch
			{
				Language.De => "Kanalbenachrichtigung",
				Language.Fr => "Notification de chaîne",
				Language.Es => "Notificación de canal",
				Language.Ru => "Уведомление канала",
				Language.It => "Notifica del canale",
				_ => "Channel notification"
			},
			AutoModChannelNotificationBehavior.SendNotificationAndDelete => PreferredLanguage switch
			{
				Language.De => "Temporäre Kanalbenachrichtigung",
				Language.Fr => "Notification de chaîne temporaire",
				Language.Es => "Notificación de canal temporal",
				Language.Ru => "Уведомление о временном канале",
				Language.It => "Notifica temporanea del canale",
				_ => "Temporary channel notification"
			},
			AutoModChannelNotificationBehavior.NoNotification => PreferredLanguage switch
			{
				Language.De => "Keine Kanalbenachrichtigung",
				Language.Fr => "Aucune notification de chaîne",
				Language.Es => "Sin notificación de canal",
				Language.Ru => "Уведомление о канале отсутствует",
				Language.It => "Nessuna notifica del canale",
				_ => "No channel notification"
			},
			_ => "Unknown"
		};
	}
}