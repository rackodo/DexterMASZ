using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Punishments.Models;

namespace Punishments.Translators;

public class PunishmentNotificationTranslator : Translator
{
	public string NotificationModCaseCommentsShortCreate()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kommentar erstellt",
			Language.Fr => "Commentaire créé",
			Language.Es => "Comentario creado",
			Language.Ru => "Комментарий создан",
			Language.It => "Commento creato",
			_ => "Comment created"
		};
	}

	public string NotificationModCaseCommentsShortUpdate()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kommentar aktualisiert",
			Language.Fr => "Commentaire mis à jour",
			Language.Es => "Comentario actualizado",
			Language.Ru => "Комментарий обновлен",
			Language.It => "Commento aggiornato",
			_ => "Comment updated"
		};
	}

	public string NotificationModCaseCommentsShortDelete()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kommentar gelöscht",
			Language.Fr => "Commentaire supprimé",
			Language.Es => "Comentario borrado",
			Language.Ru => "Комментарий удален",
			Language.It => "Commento cancellato",
			_ => "Comment deleted"
		};
	}

	public string NotificationModCaseCommentsCreate(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Ein **Kommentar** wurde von <@{actor.Id}> erstellt.",
			Language.Fr => $"Un **commentaire** a été créé par <@{actor.Id}>.",
			Language.Es => $"<@{actor.Id}> ha creado un **comentario**.",
			Language.Ru => $"**комментарий** был создан <@{actor.Id}>.",
			Language.It => $"Un **commento** è stato creato da <@{actor.Id}>.",
			_ => $"A **comment** has been created by <@{actor.Id}>."
		};
	}

	public string NotificationModCaseCommentsUpdate(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Ein **Kommentar** wurde von <@{actor.Id}> aktualisiert.",
			Language.Fr => $"Un **commentaire** a été mis à jour par <@{actor.Id}>.",
			Language.Es => $"<@{actor.Id}> ha actualizado un **comentario **.",
			Language.Ru => $"**комментарий ** был обновлен пользователем <@{actor.Id}>.",
			Language.It => $"Un **commento** è stato aggiornato da <@{actor.Id}>.",
			_ => $"A **comment** has been updated by <@{actor.Id}>."
		};
	}

	public string NotificationModCaseCommentsDelete(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Ein **Kommentar** wurde von <@{actor.Id}> gelöscht.",
			Language.Fr => $"Un **commentaire** a été supprimé par <@{actor.Id}>.",
			Language.Es => $"<@{actor.Id}> ha eliminado un **comentario **.",
			Language.Ru => $"**комментарий** был удален <@{actor.Id}>.",
			Language.It => $"Un **commento** è stato eliminato da <@{actor.Id}>.",
			_ => $"A **comment** has been deleted by <@{actor.Id}>."
		};
	}

	public string NotificationModCaseFileCreate(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Eine **Datei** wurde von <@{actor.Id}> ({actor.Username}#{actor.Discriminator}) hochgeladen.",
			Language.Fr =>
				$"Un **fichier** a été téléchargé par <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.Es => $"<@{actor.Id}> ({actor.Username}#{actor.Discriminator} ha subido un **archivo**).",
			Language.Ru =>
				$"**файл** был загружен пользователем <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.It => $"Un **file** è stato caricato da <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			_ => $"A **file** has been uploaded by <@{actor.Id}> ({actor.Username}#{actor.Discriminator})."
		};
	}

	public string NotificationModCaseFileDelete(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Eine **Datei** wurde von <@{actor.Id}> ({actor.Username}#{actor.Discriminator}) gelöscht.",
			Language.Fr => $"Un **fichier** a été supprimé par <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.Es => $"<@{actor.Id}> ({actor.Username}#{actor.Discriminator}) ha eliminado un **archivo**.",
			Language.Ru => $"**файл** был удален <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.It => $"Un **file** è stato eliminato da <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			_ => $"A **file** has been deleted by <@{actor.Id}> ({actor.Username}#{actor.Discriminator})."
		};
	}

	public string NotificationModCaseFileUpdate(IUser actor)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Eine **Datei** wurde von <@{actor.Id}> ({actor.Username}#{actor.Discriminator}) aktualisiert.",
			Language.Fr =>
				$"Un **fichier** a été mis à jour par <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.Es => $"<@{actor.Id}> ({actor.Username}#{actor.Discriminator}) ha actualizado un **archivo**.",
			Language.Ru => $"**файл** был обновлен <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			Language.It => $"Un **file** è stato aggiornato da <@{actor.Id}> ({actor.Username}#{actor.Discriminator}).",
			_ => $"A **file** has been updated by <@{actor.Id}> ({actor.Username}#{actor.Discriminator})."
		};
	}

	public string NotificationModCaseDmWarn(IGuild guild, string serviceBaseUrl)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Die Moderatoren von `{guild.Name}` haben dich verwarnt.\nFür weitere Informationen besuche: {serviceBaseUrl}",
			Language.Fr =>
				$"Les modérateurs de la guilde `{guild.Name}` vous ont prévenu.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
			Language.Es =>
				$"Los moderadores del gremio `{guild.Name}` te han advertido.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
			Language.Ru =>
				$"Модераторы гильдии `{guild.Name}` вас предупредили.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
			Language.It =>
				$"I moderatori della gilda `{guild.Name}` ti hanno avvisato.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
			_ =>
				$"The moderators of guild `{guild.Name}` have warned you.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
		};
	}

	public string NotificationModCaseDmMutePerm(IGuild guild, string serviceBaseUrl)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Die Moderatoren von `{guild.Name}` haben dich stummgeschalten.\nFür weitere Informationen besuche: {serviceBaseUrl}",
			Language.Fr =>
				$"Les modérateurs de la guilde `{guild.Name}` vous ont mis en sourdine.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
			Language.Es =>
				$"Los moderadores del gremio `{guild.Name}` te han silenciado.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
			Language.Ru =>
				$"Модераторы гильдии `{guild.Name}` отключили вас.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
			Language.It =>
				$"I moderatori della gilda `{guild.Name}` ti hanno disattivato.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
			_ =>
				$"The moderators of guild `{guild.Name}` have muted you.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
		};
	}

	public string NotificationModCaseDmBanPerm(IGuild guild, string serviceBaseUrl)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Die Moderatoren von `{guild.Name}` haben dich gebannt.\nFür weitere Informationen besuche: {serviceBaseUrl}",
			Language.Fr =>
				$"Les modérateurs de la guilde `{guild.Name}` vous ont banni.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
			Language.Es =>
				$"Los moderadores del gremio `{guild.Name}` te han prohibido.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
			Language.Ru =>
				$"Модераторы гильдии `{guild.Name}` забанили вас.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
			Language.It =>
				$"I moderatori della gilda `{guild.Name}` ti hanno bannato.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
			_ =>
				$"The moderators of guild `{guild.Name}` have banned you.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
		};
	}

	public string NotificationModCaseDmKick(IGuild guild, string serviceBaseUrl)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Die Moderatoren von `{guild.Name}` haben dich kickt.\nFür weitere Informationen besuche: {serviceBaseUrl}",
			Language.Fr =>
				$"Les modérateurs de la guilde `{guild.Name}` vous ont viré.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
			Language.Es =>
				$"Los moderadores del gremio `{guild.Name}` te han pateado.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
			Language.Ru =>
				$"Модераторы гильдии `{guild.Name}` выгнали вас.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
			Language.It =>
				$"I moderatori della gilda `{guild.Name}` ti hanno espulso.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
			_ =>
				$"The moderators of guild `{guild.Name}` have kicked you.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
		};
	}

	public string NotificationDiscordAuditLogPunishmentsExecute(int caseId, ulong modId, string reason)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Bestrafung für vorfall #{caseId} durch Moderator {modId} ausgeführt: \"{reason}\"",
			Language.Fr => $"Punition pour mod case #{caseId} par le modérateur {modId} exécutée : \"{reason}\"",
			Language.Es => $"Castigo por mod case # {caseId} por el moderador {modId} ejecutado: \"{reason} \"",
			Language.Ru => $"Наказание за mod case # {caseId} модератором {modId} выполнено: \"{reason} \"",
			Language.It => $"Punizione per mod case #{caseId} eseguita dal moderatore {modId}: \"{reason}\"",
			_ => $"Punishment for mod case #{caseId} by moderator {modId} executed: \"{reason}\""
		};
	}

	public string NotificationDiscordAuditLogPunishmentsUndone(int caseId, string reason)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Bestrafung für vorfall #{caseId} rückgängig gemacht: \"{reason}\"",
			Language.Fr => $"Punition pour mod case #{caseId} annulée : \"{reason}\"",
			Language.Es => $"Castigo por mod case # {caseId} deshecho: \"{reason} \"",
			Language.Ru => $"Наказание за mod case # {caseId} отменено: \"{reason} \"",
			Language.It => $"Punizione per mod case #{caseId} annullata: \"{reason}\"",
			_ => $"Punishment for mod case #{caseId} undone: \"{reason}\""
		};
	}

	public string NotificationModCaseDmBanTemp(ModCase modCase, IGuild guild, string serviceBaseUrl)
	{
		if (modCase.PunishedUntil != null)
			return PreferredLanguage switch
			{
				Language.De =>
					$"Die moderatoren von `{guild.Name}` haben dich temporär gebannt bis {modCase.PunishedUntil.Value.ToDiscordTs()}.\nFür weitere Informationen besuche: {serviceBaseUrl}",
				Language.Fr =>
					$"Les modérateurs de la guilde `{guild.Name}` vous ont temporairement banni jusqu'à {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
				Language.Es =>
					$"Los moderadores del gremio `{guild.Name}` te han baneado temporalmente hasta el {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
				Language.Ru =>
					$"Модераторы гильдии `{guild.Name}` временно заблокировали вас до {modCase.PunishedUntil.Value.ToDiscordTs()}.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
				Language.It =>
					$"I moderatori della gilda `{guild.Name}` ti hanno temporaneamente bannato fino al {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
				_ =>
					$"The moderators of guild `{guild.Name}` have temporarily banned you until {modCase.PunishedUntil.Value.ToDiscordTs()}.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
			};
		return string.Empty;
	}

	public string NotificationModCaseDmMuteTemp(ModCase modCase, IGuild guild, string serviceBaseUrl)
	{
		if (modCase.PunishedUntil != null)
			return PreferredLanguage switch
			{
				Language.De =>
					$"Die moderatoren von `{guild.Name}` haben dich temporär stummgeschalten bis {modCase.PunishedUntil.Value.ToDiscordTs()}.\nFür weitere Informationen besuche: {serviceBaseUrl}",
				Language.Fr =>
					$"Les modérateurs de la guilde `{guild.Name}` vous ont temporairement mis en sourdine jusqu'à {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPour plus d'informations ou pour une réhabilitation, visitez : {serviceBaseUrl}",
				Language.Es =>
					$"Los moderadores del gremio `{guild.Name}` te han silenciado temporalmente hasta {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPara obtener más información o rehabilitación, visite: {serviceBaseUrl}",
				Language.Ru =>
					$"Модераторы гильдии `{guild.Name}` временно отключили ваш звук до {modCase.PunishedUntil.Value.ToDiscordTs()}.\nДля получения дополнительной информации или реабилитации посетите: {serviceBaseUrl}",
				Language.It =>
					$"I moderatori della gilda `{guild.Name}` ti hanno temporaneamente disattivato l'audio fino a {modCase.PunishedUntil.Value.ToDiscordTs()}.\nPer maggiori informazioni o visita riabilitativa: {serviceBaseUrl}",
				_ =>
					$"The moderators of guild `{guild.Name}` have temporarily muted you until {modCase.PunishedUntil.Value.ToDiscordTs()}.\nFor more information or rehabilitation visit: {serviceBaseUrl}"
			};
		return string.Empty;
	}

	public string NotificationModCase(ModCase modCase, IUser moderator)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Ein **vorfall** für <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) wurde von <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}) erstellt.",
			Language.Fr =>
				$"Un **mod case** pour <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) a été créé par <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Es =>
				$"Un **mod case** para <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) ha sido creado por <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Ru =>
				$"**Modcase** для <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) был создан <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.It =>
				$"Un **mod case** per <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) è stato creato da <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			_ =>
				$"A **mod case** for <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) has been created by <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator})."
		};
	}

	public string NotificationModCaseUpdate(ModCase modCase, IUser moderator)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Ein **vorfall** für <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) wurde von <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}) aktualisiert.",
			Language.Fr =>
				$"Un **mod case** pour <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) a été mis à jour par <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Es =>
				$"Un **mod case** para <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) ha sido actualizado por <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Ru =>
				$"**Modcase** для <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) был обновлен <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.It =>
				$"Un **mod case** per <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) è stato aggiornato da <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			_ =>
				$"A **mod case** for <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) has been updated by <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator})."
		};
	}

	public string NotificationModCaseDelete(ModCase modCase, IUser moderator)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Ein **vorfall** für <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) wurde von <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}) gelöscht.",
			Language.Fr =>
				$"Un **mod case** pour <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) a été supprimé par <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Es =>
				$"Un **mod case** para <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) ha sido eliminado por <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.Ru =>
				$"**Modcase** для <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) был удален <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			Language.It =>
				$"Un **mod case** per <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) è stato eliminato da <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator}).",
			_ =>
				$"A **mod case** for <@{modCase.UserId}> ({modCase.Username}#{modCase.Discriminator}) has been deleted by <@{moderator.Id}> ({moderator.Username}#{moderator.Discriminator})."
		};
	}
}