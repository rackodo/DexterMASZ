using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.GuildAudits.Translators;

public class GuildAuditNotificationTranslator : Translator
{
	public string NotificationGuildAuditMentionRoles()
	{
		return PreferredLanguage switch
		{
			Language.De => "Rolle(n) erwähnen",
			Language.At => "Rolle(n) erwähnan",
			Language.Fr => "Mentionner le(s) rôle(s)",
			Language.Es => "Mencionar rol (s)",
			Language.Ru => "Упоминание ролей",
			Language.It => "Menzione ruolo/i",
			_ => "Mention role(s)"
		};
	}

	public string NotificationGuildAuditTitle()
	{
		return PreferredLanguage switch
		{
			Language.De => "Gildenspezifischer Audit-Log",
			Language.At => "Güdnspezifischa Audit-Log",
			Language.Fr => "Journal d'audit au niveau de la guilde",
			Language.Es => "Registro de auditoría a nivel de gremio",
			Language.Ru => "Журнал аудита на уровне гильдии",
			Language.It => "Registro di controllo a livello di gilda",
			_ => "Guild-level audit log"
		};
	}

	public string NotificationGuildAuditInternalCreate(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis `{eventName}` wurde von {actor.Mention} eingerichtet.",
			Language.At =>
				$"Güdnspezifischa Audit-Log fias Ereignis `{eventName}` wuad vo {actor.Mention} eingrichtet.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement `{eventName}` a été mis en place par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha configurado el registro de auditoría a nivel de gremio para el evento `{eventName}`.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события `{eventName}` был создан {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento `{eventName}` è stato impostato da {actor.Mention}.",
			_ => $"Guild-level audit log for event `{eventName}` has been set up by {actor.Mention}."
		};
	}

	public string NotificationGuildAuditInternalUpdate(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis `{eventName}` wurde von {actor.Mention} bearbeitet.",
			Language.At => $"Güdnspezifischa Audit-Log fias Ereignis `{eventName}` wuad vo {actor.Mention} beoarbeit.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement `{eventName}` a été modifié par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha editado el registro de auditoría a nivel de gremio para el evento `{eventName}`.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события `{eventName}` отредактировал {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento `{eventName}` è stato modificato da {actor.Mention}.",
			_ => $"Guild-level audit log for event `{eventName}` has been edited by {actor.Mention}."
		};
	}

	public string NotificationGuildAuditInternalDelete(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis `{eventName}` wurde von {actor.Mention} gelöscht.",
			Language.At => $"Güdnspezifischa Audit-Log fias Ereignis `{eventName}` wuad vo {actor.Mention} glescht.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement `{eventName}` a été supprimé par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha eliminado el registro de auditoría a nivel de hermandad para el evento `{eventName}`.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события `{eventName}` был удален {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento `{eventName}` è stato eliminato da {actor.Mention}.",
			_ => $"Guild-level audit log for event `{eventName}` has been deleted by {actor.Mention}."
		};
	}
}