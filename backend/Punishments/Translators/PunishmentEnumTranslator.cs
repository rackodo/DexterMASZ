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
				Language.At => "Unbekonnt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			LockedCommentStatus.Locked => PreferredLanguage switch
			{
				Language.De => "Gesperrt",
				Language.At => "Gsperrt",
				Language.Fr => "Fermé à clé",
				Language.Es => "Bloqueado",
				Language.Ru => "Заблокировано",
				Language.It => "bloccato",
				_ => "Locked"
			},
			LockedCommentStatus.Unlocked => PreferredLanguage switch
			{
				Language.De => "Entsperrt",
				Language.At => "Entsperrt",
				Language.Fr => "Débloqué",
				Language.Es => "Desbloqueado",
				Language.Ru => "Разблокирован",
				Language.It => "sbloccato",
				_ => "Unlocked"
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
				Language.At => "Unbekonnt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			MarkedToDeleteStatus.Marked => PreferredLanguage switch
			{
				Language.De => "Zu löschen markiert",
				Language.At => "Zum löschn markiat",
				Language.Fr => "Marqué à supprimer",
				Language.Es => "Marcado para eliminar",
				Language.Ru => "Отмечено для удаления",
				Language.It => "Contrassegnato per eliminare",
				_ => "Marked to delete"
			},
			MarkedToDeleteStatus.Unmarked => PreferredLanguage switch
			{
				Language.De => "Nicht zu löschen markiert",
				Language.At => "Ned zum löschn markiat",
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
				Language.At => "Unbekonnt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			PunishmentActiveStatus.Active => PreferredLanguage switch
			{
				Language.De => "Aktiv",
				Language.At => "Aktiv",
				Language.Fr => "actif",
				Language.Es => "Activo",
				Language.Ru => "Активный",
				Language.It => "Attivo",
				_ => "Active"
			},
			PunishmentActiveStatus.Inactive => PreferredLanguage switch
			{
				Language.De => "Inaktiv",
				Language.At => "Inaktiv",
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
				Language.At => "Default",
				Language.Fr => "Défaut",
				Language.Es => "Defecto",
				Language.Ru => "Дефолт",
				Language.It => "Predefinito",
				_ => "Default"
			},
			CaseCreationType.AutoMod => PreferredLanguage switch
			{
				Language.De => "Automoderiert.",
				Language.At => "Automodariat.",
				Language.Fr => "Le cas est automodéré.",
				Language.Es => "El caso está autoderado.",
				Language.Ru => "Корпус автоматический.",
				Language.It => "Il caso è moderato automaticamente.",
				_ => "Case is automoderated."
			},
			CaseCreationType.Imported => PreferredLanguage switch
			{
				Language.De => "Importiert.",
				Language.At => "Importiat",
				Language.Fr => "Le cas est importé.",
				Language.Es => "El caso es importado.",
				Language.Ru => "Корпус импортный.",
				Language.It => "Il caso è importato.",
				_ => "Case is imported."
			},
			CaseCreationType.ByCommand => PreferredLanguage switch
			{
				Language.De => "Durch Befehl erstellt.",
				Language.At => "Durch an Beföh erstöt.",
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
				Language.At => "Stummschoitung",
				Language.Fr => "Muet",
				Language.Es => "Silencio",
				Language.Ru => "Немой",
				Language.It => "Muto",
				_ => "Mute"
			},
			PunishmentType.Ban => PreferredLanguage switch
			{
				Language.De => "Bann",
				Language.At => "Rauswuaf",
				Language.Fr => "Interdire",
				Language.Es => "Prohibición",
				Language.Ru => "Запретить",
				Language.It => "Bandire",
				_ => "Ban"
			},
			PunishmentType.Kick => PreferredLanguage switch
			{
				Language.De => "Kick",
				Language.At => "Tritt",
				Language.Fr => "Coup",
				Language.Es => "Patear",
				Language.Ru => "Пинать",
				Language.It => "Calcio",
				_ => "Kick"
			},
			PunishmentType.Warn => PreferredLanguage switch
			{
				Language.De => "Verwarnung",
				Language.At => "Verwoarnt",
				Language.Fr => "Avertir",
				Language.Es => "Advertir",
				Language.Ru => "Предупреждать",
				Language.It => "Avvisare",
				_ => "Warn"
			},
			_ => "Unknown"
		};
	}
}