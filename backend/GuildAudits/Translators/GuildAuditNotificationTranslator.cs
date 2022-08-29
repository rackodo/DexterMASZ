using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace GuildAudits.Translators;

public class GuildAuditNotificationTranslator : Translator
{
	public string NotificationGuildAuditLogMentionRoles()
	{
		return PreferredLanguage switch
		{
			Language.De => "Rolle(n) erwähnen",
			Language.Fr => "Mentionner le(s) rôle(s)",
			Language.Es => "Mencionar rol (s)",
			Language.Ru => "Упоминание ролей",
			Language.It => "Menzione ruolo/i",
			_ => "Mention role(s)"
		};
	}

	public string NotificationGuildAuditLogExcludeRoles()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ausgenommene Rollen",
			Language.Fr => "Exclure les rôles",
			Language.Es => "Excluir roles",
			Language.Ru => "Исключить роли",
			Language.It => "Escludi ruoli",
			_ => "Exclude roles",
		};
	}
	public string NotificationGuildAuditLogExcludeChannels()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ausgenommene Kanäle",
			Language.Fr => "Exclure les chaînes",
			Language.Es => "Excluir canales",
			Language.Ru => "Исключить каналы",
			Language.It => "Escludi canali",
			_ => "Exclude channels",
		};
	}

	public string NotificationGuildAuditTitle()
	{
		return PreferredLanguage switch
		{
			Language.De => "Gildenprüfung",
			Language.Fr => "Audit De Guilde",
			Language.Es => "Auditoría Del Gremio",
			Language.Ru => "Аудит гильдии",
			Language.It => "Audit Della Gilda",
			_ => "Guild Audit"
		};
	}

	public string NotificationGuildAuditInternalCreate(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis **{eventName}** wurde von {actor.Mention} eingerichtet.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement **{eventName}** a été mis en place par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha configurado el registro de auditoría a nivel de gremio para el evento **{eventName}**.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события **{eventName}** был создан {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento **{eventName}** è stato impostato da {actor.Mention}.",
			_ => $"Guild audit for event **{eventName}** has been set up by {actor.Mention}."
		};
	}

	public string NotificationGuildAuditInternalUpdate(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis **{eventName}** wurde von {actor.Mention} bearbeitet.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement **{eventName}** a été modifié par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha editado el registro de auditoría a nivel de gremio para el evento **{eventName}**.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события **{eventName}** отредактировал {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento **{eventName}** è stato modificato da {actor.Mention}.",
			_ => $"Guild audit for event **{eventName}** has been edited by {actor.Mention}."
		};
	}

	public string NotificationGuildAuditInternalDelete(string eventName, IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Gildenspezifischer Audit-Log für Ereignis **{eventName}** wurde von {actor.Mention} gelöscht.",
			Language.Fr =>
				$"Le journal d'audit au niveau de la guilde pour l'événement **{eventName}** a été supprimé par {actor.Mention}.",
			Language.Es =>
				$"{actor.Mention} ha eliminado el registro de auditoría a nivel de hermandad para el evento **{eventName}**.",
			Language.Ru => $"Журнал аудита на уровне гильдии для события **{eventName}** был удален {actor.Mention}.",
			Language.It =>
				$"Il registro di controllo a livello di gilda per l'evento **{eventName}** è stato eliminato da {actor.Mention}.",
			_ => $"Guild audit for event **{eventName}** has been deleted by {actor.Mention}."
		};
	}
}
