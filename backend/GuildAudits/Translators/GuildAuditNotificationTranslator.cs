using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace GuildAudits.Translators;

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
			Language.De => "Gildenprüfung",
			Language.At => "Gildenprüfung",
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
			Language.At =>
				$"Güdnspezifischa Audit-Log fias Ereignis **{eventName}** wuad vo {actor.Mention} eingrichtet.",
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
			Language.At => $"Güdnspezifischa Audit-Log fias Ereignis **{eventName}** wuad vo {actor.Mention} beoarbeit.",
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
			Language.At => $"Güdnspezifischa Audit-Log fias Ereignis **{eventName}** wuad vo {actor.Mention} glescht.",
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
