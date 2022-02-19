using Bot.Abstractions;
using Bot.Enums;
using Messaging.Enums;

namespace Messaging.Translators;

public class MessagingEnumTranslator : Translator
{
	public string Enum(ScheduledMessageFailureReason enumValue)
	{
		return enumValue switch
		{
			ScheduledMessageFailureReason.Unknown => PreferredLanguage switch
			{
				Language.De => "Unbekannter Fehler",
				Language.Fr => "Erreur inconnue",
				Language.Es => "Error desconocido",
				Language.Ru => "Неизвестная ошибка",
				Language.It => "Errore sconosciuto",
				_ => "Unknown error",
			},
			ScheduledMessageFailureReason.ChannelNotFound => PreferredLanguage switch
			{
				Language.De => "Kanal nicht gefunden",
				Language.Fr => "Canal introuvable",
				Language.Es => "Canal no encontrado",
				Language.Ru => "Канал не найден",
				Language.It => "Canale non trovato",
				_ => "Channel not found",
			},
			ScheduledMessageFailureReason.InsufficientPermission => PreferredLanguage switch
			{
				Language.De => "Unzureichende Berechtigung",
				Language.Fr => "Permission insuffisante",
				Language.Es => "Permiso insuficiente",
				Language.Ru => "Недостаточно прав",
				Language.It => "Permessi insufficienti",
				_ => "Insufficient permission",
			},
			_ => "Unknown",
		};
	}
	public string Enum(ScheduledMessageStatus enumValue)
	{
		return enumValue switch
		{
			ScheduledMessageStatus.Pending => PreferredLanguage switch
			{
				Language.De => "Ausstehend",
				Language.Fr => "En attente",
				Language.Es => "Pendiente",
				Language.Ru => "Ожидается",
				Language.It => "In attesa",
				_ => "Pending",
			},
			ScheduledMessageStatus.Sent => PreferredLanguage switch
			{
				Language.De => "Gesendet",
				Language.Fr => "Envoyé",
				Language.Es => "Enviado",
				Language.Ru => "Отправлено",
				Language.It => "Inviato",
				_ => "Sent",
			},
			ScheduledMessageStatus.Failed => PreferredLanguage switch
			{
				Language.De => "Fehlgeschlagen",
				Language.Fr => "Échec",
				Language.Es => "Falló",
				Language.Ru => "Не удалось отправить",
				Language.It => "Invio fallito",
				_ => "Failed",
			},
			_ => "Unknown",
		};
	}
}
