using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Utilities.Translators;

public class UtilityFeaturesTranslator : Translator
{
	public string CmdFeaturesKickPermissionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kick-Berechtigung erteilt.",
			Language.At => "Kick-Berechtigung erteit.",
			Language.Fr => "Autorisation de kick accordée.",
			Language.Es => "Permiso de patada concedido.",
			Language.Ru => "Разрешение на удар предоставлено.",
			Language.It => "Autorizzazione calci concessa.",
			_ => "Kick permission granted."
		};
	}

	public string CmdFeaturesKickPermissionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kick-Berechtigung nicht erteilt.",
			Language.At => "Kick-Berechtigung ned erteit.",
			Language.Fr => "L'autorisation de kick n'est pas accordée.",
			Language.Es => "Permiso de patada no concedido.",
			Language.Ru => "Разрешение на удар не предоставлено.",
			Language.It => "Autorizzazione calcio non concessa.",
			_ => "Kick permission not granted."
		};
	}

	public string CmdFeaturesBanPermissionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ban-Berechtigung erteilt.",
			Language.At => "Ban-Berechtigung erteit.",
			Language.Fr => "Autorisation d'interdiction accordée.",
			Language.Es => "Prohibición concedida.",
			Language.Ru => "Получено разрешение на запрет.",
			Language.It => "Autorizzazione al divieto concessa.",
			_ => "Ban permission granted."
		};
	}

	public string CmdFeaturesBanPermissionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ban-Berechtigung nicht erteilt.",
			Language.At => "Ban-Berechtigung ned erteit.",
			Language.Fr => "Autorisation d'interdiction non accordée.",
			Language.Es => "Prohibir permiso no concedido.",
			Language.Ru => "Разрешение на запрет не предоставлено.",
			Language.It => "Autorizzazione al divieto non concessa.",
			_ => "Ban permission not granted."
		};
	}

	public string CmdFeaturesManageRolePermissionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Manage-Rolle-Berechtigung erteilt.",
			Language.At => "Manage-Rolle-Berechtigung ereit.",
			Language.Fr => "Gérer l'autorisation de rôle accordée.",
			Language.Es => "Administrar el permiso de función otorgado.",
			Language.Ru => "Разрешение на управление ролью предоставлено.",
			Language.It => "Gestire l'autorizzazione del ruolo concessa.",
			_ => "Manage role permission granted."
		};
	}

	public string CmdFeaturesManageRolePermissionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Manage-Rolle-Berechtigung nicht erteilt.",
			Language.At => "Manage-Rolle-Berechtigung ned erteit.",
			Language.Fr => "L'autorisation de gestion du rôle n'est pas accordée.",
			Language.Es => "Administrar el permiso de función no concedido.",
			Language.Ru => "Не предоставлено разрешение на управление ролью.",
			Language.It => "Autorizzazione di gestione del ruolo non concessa.",
			_ => "Manage role permission not granted."
		};
	}

	public string CmdFeaturesPunishmentExecution()
	{
		return PreferredLanguage switch
		{
			Language.De => "Bestrafungsverwaltung",
			Language.At => "Bestrofungsverwoitung",
			Language.Fr => "Exécution de la peine",
			Language.Es => "Ejecución del castigo",
			Language.Ru => "Казнь",
			Language.It => "Esecuzione della punizione",
			_ => "Punishment execution"
		};
	}

	public string CmdFeaturesPunishmentExecutionDescription()
	{
		return PreferredLanguage switch
		{
			Language.De => "Lass MASZ die Bestrafungen verwalten (z.B. temporäre Banns, Stummschaltungen, etc.).",
			Language.At => "Loss MASZ de Bestrofungen verwoitn (z.B. temporäre Banns, Stummschoitungen, etc.).",
			Language.Fr => "Laissez MASZ gérer les punitions (par exemple, tempbans, muets, etc.).",
			Language.Es => "Deje que MASZ maneje los castigos (por ejemplo, tempbans, mudos, etc.).",
			Language.Ru =>
				"Позвольте MASZ заниматься наказаниями (например, временным запретом, отключением звука и т. Д.).",
			Language.It => "Lascia che MASZ gestisca le punizioni (ad esempio tempban, mute, ecc.).",
			_ => "Let MASZ handle punishments (e.g. tempbans, mutes, etc.)."
		};
	}

	public string CmdFeaturesUnbanRequests()
	{
		return PreferredLanguage switch
		{
			Language.De => "Entbannungs-Anfragen",
			Language.At => "Entbannungs-Ofrogn",
			Language.Fr => "Annuler l'interdiction des demandes",
			Language.Es => "Solicitudes de anulación de la prohibición",
			Language.Ru => "Запросы на разблокировку",
			Language.It => "Riattiva richieste",
			_ => "Unban requests"
		};
	}

	public string CmdFeaturesUnbanRequestsDescriptionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Erlaubt Gebannten MASZ aufzurufen, sich ihre Fälle anzusehen und diese sie zu kommentieren.",
			Language.At => "Erlaubt ausgsperrtn MASZ aufzuruafa, sich ernane Fälle ozumschaun und de zum kommentian.",
			Language.Fr =>
				"Permet aux membres bannis de voir leurs cas et de les commenter pour les demandes de déban.",
			Language.Es =>
				"Permite a los miembros prohibidos ver sus casos y comentarlos para las solicitudes de deshabilitación.",
			Language.Ru =>
				"Позволяет заблокированным участникам просматривать свои дела и комментировать их для запросов на разблокировку.",
			Language.It => "Consente ai membri bannati di vedere i loro casi e commentarli per le richieste di sban.",
			_ => "Allows banned users to see their cases and comment on it for unban requests."
		};
	}

	public string CmdFeaturesUnbanRequestsDescriptionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Erlaubt Gebannten MASZ aufzurufen, sich ihre Fälle anzusehen und diese sie zu kommentieren.\nErteile diesem Bot die Ban-Berechtigung, um diese Funktion zu nutzen.",
			Language.At =>
				"Erlaubt ausgsperrtn MASZ aufzurufa, sich ernane Fälle ozumschaun und de zum kommentian. \nErteil dem Bot die Ban-Berechtigung, um de Funktion nutza zu kenna.",
			Language.Fr =>
				"Permet aux membres bannis de voir leurs cas et de les commenter pour les demandes de déban.\nAccordez à ce bot l'autorisation d'interdire l'utilisation de cette fonctionnalité.",
			Language.Es =>
				"Permite a los miembros prohibidos ver sus casos y comentarlos para las solicitudes de deshabilitación.\nOtorga a este bot el permiso de prohibición para usar esta función.",
			Language.Ru =>
				"Позволяет заблокированным участникам просматривать свои дела и комментировать их для запросов на разблокировку.\nПредоставьте этому боту разрешение на использование этой функции.",
			Language.It =>
				"Consente ai membri bannati di vedere i loro casi e commentarli per le richieste di sban.\nConcedi a questo bot il permesso di ban per utilizzare questa funzione.",
			_ =>
				"Allows banned users to see their cases and comment on it for unban requests.\nGrant this bot the ban permission to use this feature."
		};
	}

	public string CmdFeaturesReportCommand()
	{
		return PreferredLanguage switch
		{
			Language.De => "Melde-Befehl",
			Language.At => "Möde-Befehl",
			Language.Fr => "Commande de rapport",
			Language.Es => "Comando de informe",
			Language.Ru => "Команда отчета",
			Language.It => "Comando di rapporto",
			_ => "Report command"
		};
	}

	public string CmdFeaturesReportCommandDescriptionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erlaubt Mitgliedern, Nachrichten zu melden.",
			Language.At => "Erlaubt Mitglieda, Nochrichtn zu mödn.",
			Language.Fr => "Permet aux membres de signaler des messages.",
			Language.Es => "Permite a los miembros informar mensajes.",
			Language.Ru => "Позволяет участникам сообщать о сообщениях.",
			Language.It => "Consente ai membri di segnalare i messaggi.",
			_ => "Allows users to report messages."
		};
	}

	public string CmdFeaturesReportCommandDescriptionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Erlaubt Mitgliedern, Nachrichten zu melden.\nDefiniere einen internen Webhook, um diese Funktion zu nutzen.",
			Language.At =>
				"Erlaub Mitglieda, Nochrichtn zum mödn. \nDefinia an internen Webook, um de Funktion nutzn zum kennan.",
			Language.Fr =>
				"Permet aux membres de signaler des messages.\nDéfinissez un webhook interne pour le personnel pour utiliser cette fonctionnalité.",
			Language.Es =>
				"Permite a los miembros informar mensajes.\nDefina un webhook de personal interno para utilizar esta función.",
			Language.Ru =>
				"Позволяет участникам сообщать о сообщениях.\nОпределите внутренний веб-перехватчик персонала, чтобы использовать эту функцию.",
			Language.It =>
				"Consente ai membri di segnalare i messaggi.\nDefinire un webhook personale interno per utilizzare questa funzione.",
			_ => "Allows users to report messages.\nDefine a MASZ.Internal staff webhook to use this feature."
		};
	}

	public string CmdFeaturesInviteTracking()
	{
		return PreferredLanguage switch
		{
			Language.De => "Einladungsverfolgung",
			Language.At => "Eiladungsverfoigung",
			Language.Fr => "Suivi des invitations",
			Language.Es => "Seguimiento de invitaciones",
			Language.Ru => "Отслеживание приглашений",
			Language.It => "Invita il monitoraggio",
			_ => "Invite tracking"
		};
	}

	public string CmdFeaturesInviteTrackingDescriptionGranted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erlaubt MASZ, die Einladungen neuer Mitglieder zu verfolgen.",
			Language.At => "Erlaubt MASZ, de Eiladungen vo neichn Mitglieda zu verfoign.",
			Language.Fr => "Permet MASZ de suivre les nouveaux membres invite utilisent.",
			Language.Es =>
				"Permite a MASZ realizar un seguimiento de las invitaciones que están utilizando los nuevos miembros.",
			Language.Ru => "Позволяет MASZ отслеживать приглашения, которые используют новые участники.",
			Language.It => "Consente a MASZ di tenere traccia degli inviti utilizzati dai nuovi membri.",
			_ => "Allows MASZ to track the invites new users are using."
		};
	}

	public string CmdFeaturesInviteTrackingDescriptionNotGranted()
	{
		return PreferredLanguage switch
		{
			Language.De =>
				"Erlaubt MASZ, die Einladungen neuer Mitglieder zu verfolgen.\nErteile diesem Bot die Verwalten-Gilden-Berechtigung, um diese Funktion zu nutzen.",
			Language.At =>
				"Erlaubt MASZ, de Eiladungen vo neichn Mitglieda zu verfoign.\nErteil dem Bot die Verwoitn-Gilden-Berechtigung, um de Funktion nutzn zu kenna.",
			Language.Fr =>
				"Permet à MASZ de suivre les invitations que les nouveaux membres utilisent.\nAccordez à ce bot l'autorisation de gestion de guilde pour utiliser cette fonctionnalité.",
			Language.Es =>
				"Permite a MASZ realizar un seguimiento de las invitaciones que están utilizando los nuevos miembros.\nOtorga a este bot el permiso de gestión del gremio para usar esta función.",
			Language.Ru =>
				"Позволяет MASZ отслеживать приглашения, которые используют новые участники.\nПредоставьте этому боту разрешение на управление гильдией на использование этой функции.",
			Language.It =>
				"Consente a MASZ di tenere traccia degli inviti utilizzati dai nuovi membri.\nConcedi a questo bot il permesso di gestione della gilda per utilizzare questa funzione.",
			_ =>
				"Allows MASZ to track the invites new users are using.\nGrant this bot the manage guild permission to use this feature."
		};
	}

	public string CmdFeaturesSupportAllFeatures()
	{
		return PreferredLanguage switch
		{
			Language.De => "Dein Bot auf diesem Server ist richtig konfiguriert.",
			Language.At => "Dei Bot auf dem Serva is richtig konfiguriat.",
			Language.Fr => "Votre bot sur cette guilde est correctement configuré.",
			Language.Es => "Tu bot en este gremio está configurado correctamente.",
			Language.Ru => "Ваш бот в этой гильдии настроен правильно.",
			Language.It => "Il tuo bot in questa gilda è configurato correttamente.",
			_ => "Your bot on this guild is configured correctly."
		};
	}

	public string CmdFeaturesSupportAllFeaturesDesc()
	{
		return PreferredLanguage switch
		{
			Language.De => "Alle Funktionen von MASZ können genutzt werden.",
			Language.At => "Olle Funktionen vo MASZ kennen gnutzt wean.",
			Language.Fr => "Toutes les fonctionnalités de MASZ peuvent être utilisées.",
			Language.Es => "Se pueden utilizar todas las funciones de MASZ.",
			Language.Ru => "Можно использовать все возможности MASZ.",
			Language.It => "Tutte le funzionalità di MASZ possono essere utilizzate.",
			_ => "All features of MASZ can be used."
		};
	}

	public string CmdFeaturesMissingFeatures()
	{
		return PreferredLanguage switch
		{
			Language.De => "Es gibt Funktionen von MASZ, die du jetzt nicht nutzen kannst.",
			Language.At => "Es gibt Funktionen vo MASZ, die du jetzt ned nutzn konnst.",
			Language.Fr => "Il y a des fonctionnalités de MASZ que vous ne pouvez pas utiliser pour le moment.",
			Language.Es => "Hay funciones de MASZ que no puede utilizar en este momento.",
			Language.Ru => "Есть функции MASZ, которые вы не можете использовать прямо сейчас.",
			Language.It => "Ci sono funzionalità di MASZ che non puoi usare in questo momento.",
			_ => "There are features of MASZ that you cannot use right now."
		};
	}
}