using Bot.Abstractions;
using Bot.Enums;
using Punishments.Enums;

namespace Punishments.Translators;

public class PunishmentEnumTranslator : Translator
{
	public string Enum(LockedCommentStatus enumValue)
	{
		return enumValue switch
		{
			LockedCommentStatus.None => PreferredLanguage switch
			{
				Language.De => "Unbestimmt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			LockedCommentStatus.Locked => PreferredLanguage switch
			{
				Language.De => "Gesperrt",
				Language.Fr => "Fermé à clé",
				Language.Es => "Bloqueado",
				Language.Ru => "Заблокировано",
				Language.It => "bloccato",
				_ => "Locked"
			},
			LockedCommentStatus.Unlocked => PreferredLanguage switch
			{
				Language.De => "Entsperrt",
				Language.Fr => "Débloqué",
				Language.Es => "Desbloqueado",
				Language.Ru => "Разблокирован",
				Language.It => "sbloccato",
				_ => "Unlocked"
			},
			_ => "Unknown"
		};
	}

	public string Enum(AnnouncementResult enumValue)
	{
		return enumValue switch
		{
			AnnouncementResult.Announced => PreferredLanguage switch
			{
				Language.De => "Erfolgreich abgeschlossen",
				Language.Fr => "Complété avec succès",
				Language.Es => "Completado con éxito",
				Language.Ru => "Успешно завершено",
				Language.It => "Completato con successo",
				_ => "Successfully completed"
			},
			AnnouncementResult.Failed => PreferredLanguage switch
			{
				Language.De => "Senden fehlgeschlagen",
				Language.Fr => "Échec de l'envoi",
				Language.Es => "Fallo al enviar",
				Language.Ru => "Не удалось отправить",
				Language.It => "Impossibile inviare",
				_ => "Failed to send"
			},
			_ => "Unknown"
		};
	}

	public string Enum(SeverityType enumValue)
	{
		return enumValue switch
		{
			SeverityType.None => PreferredLanguage switch
			{
				Language.De => "Unbestimmt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			SeverityType.Low => PreferredLanguage switch
			{
				Language.De => "Niedrig",
				Language.Fr => "Bas",
				Language.Es => "Baja",
				Language.Ru => "Низкий",
				Language.It => "Basso",
				_ => "Low"
			},
			SeverityType.High => PreferredLanguage switch
			{
				Language.De => "Hoch",
				Language.Fr => "Haute",
				Language.Es => "Alta",
				Language.Ru => "Высокая",
				Language.It => "Alto",
				_ => "High"
			},
			_ => "Unknown"
		};
	}

	public string Enum(MarkedToDeleteStatus enumValue)
	{
		return enumValue switch
		{
			MarkedToDeleteStatus.None => PreferredLanguage switch
			{
				Language.De => "Unbestimmt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			MarkedToDeleteStatus.Marked => PreferredLanguage switch
			{
				Language.De => "Zu löschen markiert",
				Language.Fr => "Marqué à supprimer",
				Language.Es => "Marcado para eliminar",
				Language.Ru => "Отмечено для удаления",
				Language.It => "Contrassegnato per eliminare",
				_ => "Marked to delete"
			},
			MarkedToDeleteStatus.Unmarked => PreferredLanguage switch
			{
				Language.De => "Nicht zu löschen markiert",
				Language.Fr => "Non marqué pour supprimer",
				Language.Es => "No marcado para eliminar",
				Language.Ru => "Не отмечен для удаления",
				Language.It => "Non contrassegnato per l'eliminazione",
				_ => "Not marked to delete"
			},
			_ => "Unknown"
		};
	}

	public string Enum(PunishmentActiveStatus enumValue)
	{
		return enumValue switch
		{
			PunishmentActiveStatus.None => PreferredLanguage switch
			{
				Language.De => "Unbestimmt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			PunishmentActiveStatus.Active => PreferredLanguage switch
			{
				Language.De => "Aktiv",
				Language.Fr => "actif",
				Language.Es => "Activo",
				Language.Ru => "Активный",
				Language.It => "Attivo",
				_ => "Active"
			},
			PunishmentActiveStatus.Inactive => PreferredLanguage switch
			{
				Language.De => "Inaktiv",
				Language.Fr => "Inactif",
				Language.Es => "Inactivo",
				Language.Ru => "Неактивный",
				Language.It => "Non attivo",
				_ => "Inactive"
			},
			_ => "Unknown"
		};
	}

	public string Enum(CaseCreationType enumValue)
	{
		return enumValue switch
		{
			CaseCreationType.Default => PreferredLanguage switch
			{
				Language.De => "Default",
				Language.Fr => "Défaut",
				Language.Es => "Defecto",
				Language.Ru => "Дефолт",
				Language.It => "Predefinito",
				_ => "Default"
			},
			CaseCreationType.AutoMod => PreferredLanguage switch
			{
				Language.De => "Automoderiert.",
				Language.Fr => "Le cas est automodéré.",
				Language.Es => "El caso está autoderado.",
				Language.Ru => "Корпус автоматический.",
				Language.It => "Il caso è moderato automaticamente.",
				_ => "Case is automoderated."
			},
			CaseCreationType.Imported => PreferredLanguage switch
			{
				Language.De => "Importiert.",
				Language.Fr => "Le cas est importé.",
				Language.Es => "El caso es importado.",
				Language.Ru => "Корпус импортный.",
				Language.It => "Il caso è importato.",
				_ => "Case is imported."
			},
			CaseCreationType.ByCommand => PreferredLanguage switch
			{
				Language.De => "Durch Befehl erstellt.",
				Language.Fr => "Cas créé par commande.",
				Language.Es => "Caso creado por comando.",
				Language.Ru => "Дело создано командой.",
				Language.It => "Caso creato da comando.",
				_ => "Case created by command."
			},
			_ => "Unknown"
		};
	}

	public string Enum(PunishmentType enumValue)
	{
		return enumValue switch
		{
			PunishmentType.Mute => PreferredLanguage switch
			{
				Language.De => "Stummschaltung",
				Language.Fr => "Muet",
				Language.Es => "Silencio",
				Language.Ru => "Немой",
				Language.It => "Muto",
				_ => "Mute"
			},
			PunishmentType.Ban => PreferredLanguage switch
			{
				Language.De => "Bann",
				Language.Fr => "Interdire",
				Language.Es => "Prohibición",
				Language.Ru => "Запретить",
				Language.It => "Bandire",
				_ => "Ban"
			},
			PunishmentType.Kick => PreferredLanguage switch
			{
				Language.De => "Kick",
				Language.Fr => "Coup",
				Language.Es => "Patear",
				Language.Ru => "Пинать",
				Language.It => "Calcio",
				_ => "Kick"
			},
			PunishmentType.Warn => PreferredLanguage switch
			{
				Language.De => "Verwarnung",
				Language.Fr => "Avertir",
				Language.Es => "Advertir",
				Language.Ru => "Предупреждать",
				Language.It => "Avvisare",
				_ => "Warn"
			},
			PunishmentType.FinalWarn => PreferredLanguage switch
			{
				Language.De => "Letzte Warnung",
				Language.Fr => "Dernier avertissement",
				Language.Es => "Última advertencia",
				Language.Ru => "Последнее предупреждение",
				Language.It => "Avviso finale",
				_ => "Final Warning"
			},
			_ => "Unknown"
		};
	}
}