using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace Punishments.Translators;

public class PunishmentTranslator : Translator
{
	public string Punishment()
	{
		return PreferredLanguage switch
		{
			Language.De => "Bestrafung",
			Language.At => "Bestrofung",
			Language.Fr => "Châtiment",
			Language.Es => "Castigo",
			Language.Ru => "Наказание",
			Language.It => "Punizione",
			_ => "Punishment"
		};
	}

	public string PunishedUntil()
	{
		return PreferredLanguage switch
		{
			Language.De => "Bestrafung bis",
			Language.At => "Bestroft bis",
			Language.Fr => "Puni jusqu'à",
			Language.Es => "Castigado hasta",
			Language.Ru => "Наказан до",
			Language.It => "Punito fino a",
			_ => "Punished until"
		};
	}

	public string Cases()
	{
		return PreferredLanguage switch
		{
			Language.De => "Vorfälle",
			Language.At => "Vorfälle",
			Language.Fr => "Cas",
			Language.Es => "Casos",
			Language.Ru => "Случаи",
			Language.It => "casi",
			_ => "Cases"
		};
	}

	public string NoCases()
	{
		return PreferredLanguage switch
		{
			Language.De => "Es gibt keine Fälle für diesen Benutzer.",
			Language.At => "Es gibt kane Fälle fia diesn Benutza.",
			Language.Fr => "Il n'y a pas de cas pour cet utilisateur.",
			Language.Es => "No hay casos para este usuario.",
			Language.Ru => "Для этого пользователя нет случаев.",
			Language.It => "Non ci sono casi per questo utente.",
			_ => "There are no cases for this user."
		};
	}

	public string CaseId()
	{
		return PreferredLanguage switch
		{
			Language.De => "Fall-ID",
			Language.At => "Case ID",
			Language.Fr => "ID de cas",
			Language.Es => "Identificación del caso",
			Language.Ru => "Идентификатор дела",
			Language.It => "ID Caso",
			_ => "Case ID"
		};
	}

	public string ActivePunishments()
	{
		return PreferredLanguage switch
		{
			Language.De => "Aktive Bestrafungen",
			Language.At => "Aktive Bestrofungen",
			Language.Fr => "Punitions actives",
			Language.Es => "Castigos activos",
			Language.Ru => "Активные наказания",
			Language.It => "punizioni attive",
			_ => "Active punishments"
		};
	}

	public string Imported()
	{
		return PreferredLanguage switch
		{
			Language.De => "Importiert",
			Language.At => "Importiat",
			Language.Fr => "Importé",
			Language.Es => "Importado",
			Language.Ru => "Импортный",
			Language.It => "importato",
			_ => "Imported"
		};
	}

	public string ImportedFromExistingBans()
	{
		return PreferredLanguage switch
		{
			Language.De => "Importiert aus bestehenden Sperren",
			Language.At => "Importiat aus vorhondane Sperren",
			Language.Fr => "Importé à partir des interdictions existantes",
			Language.Es => "Importado de prohibiciones existentes",
			Language.Ru => "Импортировано из существующих банов",
			Language.It => "Importato da divieti esistenti",
			_ => "Imported from existing bans"
		};
	}

	public string CaseCreated(int caseId, string caseLink)
	{
		return PreferredLanguage switch
		{
			Language.De => $"Fall `#{caseId}` erstellt: {caseLink}",
			Language.At => $"Foi `#{caseId}` erstöt: {caseLink}",
			Language.Fr => $"Cas `#{caseId}` créé : {caseLink}",
			Language.Es => $"Caso `# {caseId}` creado: {caseLink}",
			Language.Ru => $"Обращение `# {caseId}` создано: {caseLink}",
			Language.It => $"Caso `#{caseId}` creato: {caseLink}",
			_ => $"Case `#{caseId}` created: {caseLink}"
		};
	}

	public string ReportFailed()
	{
		return PreferredLanguage switch
		{
			Language.De => "Interner Benachrichtigungsversand an Moderatoren für Meldebefehl fehlgeschlagen.",
			Language.At => "Interna Benochrichtigungsvasond on de Modaratoan fian Mödebefehl fehlgschlogn.",
			Language.Fr => "Échec de l'envoi de la notification interne aux modérateurs pour la commande de rapport.",
			Language.Es => "No se pudo enviar una notificación interna a los moderadores para el comando de informe.",
			Language.Ru => "Не удалось отправить внутреннее уведомление модераторам для команды отчета.",
			Language.It => "Impossibile inviare una notifica interna ai moderatori per il comando di segnalazione.",
			_ => "Failed to send internal notification to moderators for report command."
		};
	}

	public string ReportSent()
	{
		return PreferredLanguage switch
		{
			Language.De => "Meldung gesendet.",
			Language.At => "Mödung gsendt.",
			Language.Fr => "Rapport envoyé.",
			Language.Es => "Reporte enviado.",
			Language.Ru => "Отчет отправлен.",
			Language.It => "Rapporto inviato.",
			_ => "Report sent."
		};
	}

	public string ReportContent(IUser user, IMessage message, IMentionable channel)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"{user.Mention} meldete eine Nachricht von {message.Author.Mention} in {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.At =>
				$"{user.Mention} mödet a Nochricht vo {message.Author.Mention} in {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Fr =>
				$"{user.Mention} a signalé un message de {message.Author.Mention} dans {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Es =>
				$"{user.Mention} informó un mensaje de {message.Author.Mention} en {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Ru =>
				$"{user.Mention} сообщил о сообщении от {message.Author.Mention} в {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.It =>
				$"{user.Mention} ha segnalato un messaggio da {message.Author.Mention} in {channel.Mention}.\n{message.GetJumpUrl()}",
			_ =>
				$"{user.Mention} reported a message from {message.Author.Mention} in {channel.Mention}.\n{message.GetJumpUrl()}"
		};
	}

	public string NotAllowedToViewCase()
	{
		return PreferredLanguage switch
		{
			Language.De => "Du darfst diesen Fall nicht ansehen.",
			Language.At => "Du derfst da den Foi ned oschaun.",
			Language.Fr => "Vous n'êtes pas autorisé à voir ce cas.",
			Language.Es => "No se le permite ver este caso.",
			Language.Ru => "Вам не разрешено просматривать это дело.",
			Language.It => "Non sei autorizzato a visualizzare questo caso.",
			_ => "You are not allowed to view this case."
		};
	}

	public string Result()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ergebnis",
			Language.At => "Ergebnis",
			Language.Fr => "Résultat",
			Language.Es => "Resultado",
			Language.Ru => "Результат",
			Language.It => "Risultato",
			_ => "Result"
		};
	}

	public string WaitingForApproval()
	{
		return PreferredLanguage switch
		{
			Language.De => "Warte auf Bestätigung.",
			Language.At => "Woat auf a Bestätigung",
			Language.Fr => "En attente d'approbation.",
			Language.Es => "A la espera de la aprobación.",
			Language.Ru => "Ожидание подтверждения.",
			Language.It => "In attesa di approvazione.",
			_ => "Waiting for approval."
		};
	}

	public string Canceled()
	{
		return PreferredLanguage switch
		{
			Language.De => "Abgebrochen",
			Language.At => "Wuad obbrochn",
			Language.Fr => "Annulé",
			Language.Es => "Cancelado",
			Language.Ru => "Отменено",
			Language.It => "Annullato",
			_ => "Canceled"
		};
	}

	public string PublicNotification()
	{
		return PreferredLanguage switch
		{
			Language.De => "Öffentliche Benachrichtigung",
			Language.At => "Öffentliche Benochrichtigung",
			Language.Fr => "Avis public",
			Language.Es => "Notificación pública",
			Language.Ru => "Публичное уведомление",
			Language.It => "Notifica pubblica",
			_ => "Public notification"
		};
	}

	public string ShouldSendPublicNotification()
	{
		return PreferredLanguage switch
		{
			Language.De => "Soll eine öffentliche Benachrichtigung gesendet werden?",
			Language.At => "Soll a öffentliche Benochrichtung gsendet wean?",
			Language.Fr => "Envoyer une notification publique ?",
			Language.Es => "¿Enviar una notificación pública?",
			Language.Ru => "Отправить публичное уведомление?",
			Language.It => "Inviare una notifica pubblica?",
			_ => "Send a public notification?"
		};
	}

	public string Cancel()
	{
		return PreferredLanguage switch
		{
			Language.De => "Abbrechen",
			Language.At => "Obbrechn",
			Language.Fr => "Annuler",
			Language.Es => "Cancelar",
			Language.Ru => "Отмена",
			Language.It => "Annulla",
			_ => "Cancel"
		};
	}

	public string NoPublicNotification()
	{
		return PreferredLanguage switch
		{
			Language.De => "Keine öffentliche Benachrichtigung",
			Language.At => "Ka öffentliche Benochrichtung",
			Language.Fr => "Aucune notification publique",
			Language.Es => "Sin notificación pública",
			Language.Ru => "Нет публичного уведомления",
			Language.It => "Nessuna notifica pubblica",
			_ => "No public notification"
		};
	}

	public string CreatedAt()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erstellt am.",
			Language.At => "Erstöt am.",
			Language.Fr => "Créé à.",
			Language.Es => "Creado en.",
			Language.Ru => "Создано в.",
			Language.It => "Creato a.",
			_ => "Created at."
		};
	}

	public string NoActiveModCases()
	{
		return PreferredLanguage switch
		{
			Language.De => "Keine aktiven Mod-Fälle wurden gefunden.",
			Language.At => "Kane aktiven Mod-Fälle san gfundn woan.",
			Language.Fr => "Aucun modcase actif n'a été trouvé.",
			Language.Es => "No se han encontrado casos de modulación activos.",
			Language.Ru => "Активных модкейсов не обнаружено.",
			Language.It => "Nessun modcase attivo è stato trovato.",
			_ => "No active modcases have been found."
		};
	}

	public string FoundCasesForUnmute(int caseCount)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Es wurden `{caseCount}` aktive Fälle gefunden. Möchtest du alle deaktivieren oder löschen, um den Benutzer nicht mehr stummgeschaltet zu lassen?",
			Language.At =>
				$"Es san `{caseCount}` aktive Fälle gfundn woan. Möchtest olle deaktivian oda löschn, damit da Nutza nimma stummgschoitn bleibt?",
			Language.Fr =>
				$"`{caseCount}` cas actifs trouvés. Voulez-vous les désactiver ou les supprimer tous pour réactiver le son de l'utilisateur ?",
			Language.Es =>
				$"Se encontraron casos activos `{caseCount}`. ¿Quieres desactivarlos o eliminarlos todos para dejar de silenciar al usuario?",
			Language.Ru =>
				$"Обнаружены активные обращения `{caseCount}`. Вы хотите деактивировать или удалить их все, чтобы включить микрофон для пользователя?",
			Language.It =>
				$"Trovati casi attivi di `{caseCount}`. Vuoi disattivarli o eliminarli tutti per riattivare l'audio dell'utente?",
			_ =>
				$"Found `{caseCount}` active cases. Do you want to deactivate or delete all of them to unmute the user?"
		};
	}

	public string MutesDeleted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen gelöscht",
			Language.At => "Sperrungen san glescht woan",
			Language.Fr => "Muets supprimés",
			Language.Es => "Silenciados eliminados",
			Language.Ru => "Без звука удалено",
			Language.It => "Disattiva audio cancellato",
			_ => "Mutes deleted"
		};
	}

	public string MutesDeactivated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen deaktiviert",
			Language.At => "Sperrungen san deaktiviert woan",
			Language.Fr => "Muet désactivé",
			Language.Es => "Silencios desactivados",
			Language.Ru => "Отключение звука отключено",
			Language.It => "Mute disattivate",
			_ => "Mutes deactivated"
		};
	}

	public string DeleteMutes()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen löschen",
			Language.At => "Sperrungen löschn",
			Language.Fr => "Supprimer les sourdines",
			Language.Es => "Eliminar silencios",
			Language.Ru => "Удалить отключение звука",
			Language.It => "Elimina mute",
			_ => "Delete Mutes"
		};
	}

	public string DeactivateMutes()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen deaktivieren",
			Language.At => "Sperrungen deaktivian",
			Language.Fr => "Désactiver les sourdines",
			Language.Es => "Silenciar desactivados",
			Language.Ru => "Отключить отключение звука",
			Language.It => "Disattiva sordina",
			_ => "Deativate Mutes"
		};
	}

	public string FoundCasesForUnban(int caseCount)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"Es wurden `{caseCount}` aktive Fälle gefunden. Möchtest du alle deaktivieren oder löschen, um den Benutzer entsperren zu lassen?",
			Language.At =>
				$"Es san `{caseCount}` aktive Fälle gfundn woan. Möchtest olle deaktivian oda löschn, damit da Nutza entsperrt bleibt?",
			Language.Fr =>
				$"`{caseCount}` cas actifs trouvés. Voulez-vous les désactiver ou les supprimer tous pour annuler l'interdiction de l'utilisateur ?",
			Language.Es =>
				$"Se encontraron casos activos `{caseCount}`. ¿Quieres desactivarlos o eliminarlos todos para desbloquear al usuario?",
			Language.Ru =>
				$"Обнаружены активные обращения `{caseCount}`. Вы хотите деактивировать или удалить их все, чтобы разблокировать пользователя?",
			Language.It =>
				$"Trovati casi attivi di `{caseCount}`. Vuoi disattivarli o eliminarli tutti per riabilitare l'utente?",
			_ => $"Found `{caseCount}` active cases. Do you want to deactivate or delete all of them to unban the user?"
		};
	}

	public string BansDeleted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen gelöscht",
			Language.At => "Sperrungen glescht",
			Language.Fr => "Interdictions supprimées",
			Language.Es => "Prohibiciones eliminadas",
			Language.Ru => "Баны удалены",
			Language.It => "Divieti cancellati",
			_ => "Bans deleted"
		};
	}

	public string BansDeactivated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen deaktiviert",
			Language.At => "Sperrungen deaktiviat",
			Language.Fr => "Interdictions désactivées",
			Language.Es => "Prohibiciones desactivadas",
			Language.Ru => "Баны отключены",
			Language.It => "Divieti disattivati",
			_ => "Bans deactivated"
		};
	}

	public string DeleteBans()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen löschen",
			Language.At => "Sperrungen löschn",
			Language.Fr => "Supprimer les bannissements",
			Language.Es => "Eliminar prohibiciones",
			Language.Ru => "Удалить баны",
			Language.It => "Elimina ban",
			_ => "Delete Bans"
		};
	}

	public string DeactivateBans()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sperrungen deaktivieren",
			Language.At => "Sperrungen deaktivian",
			Language.Fr => "Désactiver les interdictions",
			Language.Es => "Prohibiciones de desactivación",
			Language.Ru => "Деактивировать баны",
			Language.It => "Divieti di disattivazione",
			_ => "Deativate Bans"
		};
	}
}