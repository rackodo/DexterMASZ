using Bot.Abstractions;
using Bot.Enums;

namespace Bot.Translators;

public class BotEnumTranslator : Translator
{
	public string Enum(ApiError enumValue)
	{
		return enumValue switch
		{
			ApiError.Unknown => PreferredLanguage switch
			{
				Language.De => "Unbekannter Fehler",
				Language.Fr => "Erreur inconnue",
				Language.Es => "Error desconocido",
				Language.Ru => "Неизвестная ошибка",
				Language.It => "Errore sconosciuto",
				_ => "Unknown error"
			},
			ApiError.InvalidDiscordUser => PreferredLanguage switch
			{
				Language.De => "Ungültiger Discordbenutzer",
				Language.Fr => "Utilisateur discord invalide",
				Language.Es => "Usuario de discordia no válido",
				Language.Ru => "Недействительный пользователь Discord",
				Language.It => "Utente discord non valido",
				_ => "Invalid discord user"
			},
			ApiError.ProtectedModCaseSuspect => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt",
				Language.Fr => "L'utilisateur est protégé",
				Language.Es => "El usuario está protegido",
				Language.Ru => "Пользователь защищен",
				Language.It => "L'utente è protetto",
				_ => "User is protected"
			},
			ApiError.ProtectedModCaseSuspectIsBot => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Bot.",
				Language.Fr => "L'utilisateur est protégé. C'est un robot.",
				Language.Es => "El usuario está protegido. El es un bot.",
				Language.Ru => "Пользователь защищен. Он бот.",
				Language.It => "L'utente è protetto. Lui è un bot.",
				_ => "User is protected. He is a bot."
			},
			ApiError.ProtectedModCaseSuspectIsSiteAdmin => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Seitenadministrator.",
				Language.Fr => "L'utilisateur est protégé. Il est administrateur du site.",
				Language.Es => "El usuario está protegido. Es administrador de un sitio.",
				Language.Ru => "Пользователь защищен. Он администратор сайта.",
				Language.It => "L'utente è protetto. È un amministratore del sito.",
				_ => "User is protected. He is a site admin."
			},
			ApiError.AlreadyFinalWarned => PreferredLanguage switch
			{
				Language.De => "Dieser Benutzer ist bereits endgültig gewarnt!",
				Language.Fr => "Cet utilisateur est déjà prévenu définitivement!",
				Language.Es => "¡Este usuario ya es el último advertido!",
				Language.Ru => "Этот пользователь уже получил окончательное предупреждение!",
				Language.It => "Questo utente è già stato avvisato definitivamente!",
				_ => "This user is already final warned!"
			},
			ApiError.ProtectedModCaseSuspectIsTeam => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Teammitglied.",
				Language.Fr => "L'utilisateur est protégé. Il est membre de l'équipe.",
				Language.Es => "El usuario está protegido. Es un miembro del equipo.",
				Language.Ru => "Пользователь защищен. Он член команды.",
				Language.It => "L'utente è protetto. È un membro della squadra.",
				_ => "User is protected. He is a team user."
			},
			ApiError.ResourceNotFound => PreferredLanguage switch
			{
				Language.De => "Ressource nicht gefunden",
				Language.Fr => "Ressource introuvable",
				Language.Es => "Recurso no encontrado",
				Language.Ru => "Ресурс не найден",
				Language.It => "Risorsa non trovata",
				_ => "Resource not found"
			},
			ApiError.GuildNotFound => PreferredLanguage switch
			{
				Language.De => "Guild nicht gefunden",
				Language.Fr => "Guild introuvable",
				Language.Es => "Guild no encontrado",
				Language.Ru => "Guild не найден",
				Language.It => "Guild non trovata",
				_ => "Guild not found"
			},
			ApiError.InvalidIdentity => PreferredLanguage switch
			{
				Language.De => "Ungültige Identität",
				Language.Fr => "Identité invalide",
				Language.Es => "Identidad inválida",
				Language.Ru => "Неверная личность",
				Language.It => "Identità non valida",
				_ => "Invalid identity"
			},
			ApiError.GuildUnregistered => PreferredLanguage switch
			{
				Language.De => "Gilde ist nicht registriert",
				Language.Fr => "La guilde n'est pas enregistrée",
				Language.Es => "El gremio no está registrado",
				Language.Ru => "Гильдия не зарегистрирована",
				Language.It => "La gilda non è registrata",
				_ => "Guild is not registered. Please visit the website to set up fully."
			},
			ApiError.Unauthorized => PreferredLanguage switch
			{
				Language.De => "Nicht berechtigt",
				Language.Fr => "Non autorisé",
				Language.Es => "No autorizado",
				Language.Ru => "Неавторизованный",
				Language.It => "non autorizzato",
				_ => "Unauthorized"
			},
			ApiError.ModCaseIsMarkedToBeDeleted => PreferredLanguage switch
			{
				Language.De => "ModCase ist zum Löschen markiert",
				Language.Fr => "ModCase est marqué pour être supprimé",
				Language.Es => "ModCase está marcado para ser eliminado",
				Language.Ru => "ModCase отмечен для удаления",
				Language.It => "ModCase è contrassegnato per essere eliminato",
				_ => "ModCase is marked to be deleted"
			},
			ApiError.ModCaseIsNotMarkedToBeDeleted => PreferredLanguage switch
			{
				Language.De => "ModCase ist nicht zum Löschen markiert",
				Language.Fr => "ModCase n'est pas marqué pour être supprimé",
				Language.Es => "ModCase no está marcado para ser eliminado",
				Language.Ru => "ModCase не отмечен для удаления",
				Language.It => "ModCase non è contrassegnato per essere eliminato",
				_ => "ModCase is not marked to be deleted"
			},
			ApiError.GuildAlreadyRegistered => PreferredLanguage switch
			{
				Language.De => "Gilde ist bereits registriert",
				Language.Fr => "La guilde est déjà enregistrée",
				Language.Es => "El gremio ya está registrado",
				Language.Ru => "Гильдия уже зарегистрирована",
				Language.It => "La gilda è già registrata",
				_ => "Guild is already registered"
			},
			ApiError.RoleNotFound => PreferredLanguage switch
			{
				Language.De => "Rolle nicht gefunden",
				Language.Fr => "Rôle introuvable",
				Language.Es => "Rol no encontrado",
				Language.Ru => "Роль не найдена",
				Language.It => "Ruolo non trovato",
				_ => "Role not found"
			},
			ApiError.CannotBeSameUser => PreferredLanguage switch
			{
				Language.De => "Beide Benutzer sind gleich.",
				Language.Fr => "Les deux utilisateurs sont les mêmes.",
				Language.Es => "Ambos usuarios son iguales.",
				Language.Ru => "Оба пользователя одинаковые.",
				Language.It => "Entrambi gli utenti sono gli stessi.",
				_ => "Both users are the same."
			},
			ApiError.ResourceAlreadyExists => PreferredLanguage switch
			{
				Language.De => "Ressource existiert bereits",
				Language.Fr => "La ressource existe déjà",
				Language.Es => "El recurso ya existe",
				Language.Ru => "Ресурс уже существует",
				Language.It => "La risorsa esiste già",
				_ => "Resource already exists"
			},
			ApiError.ModCaseDoesNotAllowComments => PreferredLanguage switch
			{
				Language.De => "Kommentare sind für diesen Vorfall gesperrt",
				Language.Fr => "Les commentaires sont verrouillés pour ce modcase",
				Language.Es => "Los comentarios están bloqueados para este modcase",
				Language.Ru => "Комментарии заблокированы для этого мода",
				Language.It => "I commenti sono bloccati per questo modcase",
				_ => "Comments are locked for this modcase"
			},
			ApiError.LastCommentAlreadyFromSuspect => PreferredLanguage switch
			{
				Language.De => "Der letzte Kommentar war schon von dem Beschuldigten.",
				Language.Fr => "Le dernier commentaire était déjà du suspect.",
				Language.Es => "El último comentario ya era del sospechoso.",
				Language.Ru => "Последний комментарий уже был от подозреваемого.",
				Language.It => "L'ultimo commento era già del sospettato.",
				_ => "The last comment was already from the suspect."
			},
			ApiError.InvalidAutoModAction => PreferredLanguage switch
			{
				Language.De => "Ungültige automodsaktion",
				Language.Fr => "Action de modération automatique non valide",
				Language.Es => "Acción de automoderación no válida",
				Language.Ru => "Недопустимое действие автомодерации",
				Language.It => "Azione di moderazione automatica non valida",
				_ => "Invalid automod action"
			},
			ApiError.InvalidAutoModType => PreferredLanguage switch
			{
				Language.De => "Ungültiger automodstyp",
				Language.Fr => "Type d'automodération non valide",
				Language.Es => "Tipo de automoderación no válido",
				Language.Ru => "Неверный тип автомодерации.",
				Language.It => "Tipo di moderazione automatica non valido",
				_ => "Invalid automod type"
			},
			ApiError.TooManyTemplates => PreferredLanguage switch
			{
				Language.De => "Benutzer hat die maximale Anzahl an Templates erreicht",
				Language.Fr => "L'utilisateur a atteint la limite maximale de modèles",
				Language.Es => "El usuario ha alcanzado el límite máximo de plantillas",
				Language.Ru => "Пользователь достиг максимального предела шаблонов",
				Language.It => "L'utente ha raggiunto il limite massimo di modelli",
				_ => "User has reached the max limit of templates"
			},
			ApiError.InvalidFilePath => PreferredLanguage switch
			{
				Language.De => "Ungültiger Dateipfad",
				Language.Fr => "Chemin de fichier invalide",
				Language.Es => "Ruta de archivo no válida",
				Language.Ru => "Неверный путь к файлу",
				Language.It => "Percorso file non valido",
				_ => "Invalid file path"
			},
			ApiError.NoGuildsRegistered => PreferredLanguage switch
			{
				Language.De => "Es sind keine Gilden registriert",
				Language.Fr => "Il n'y a pas de guildes enregistrées",
				Language.Es => "No hay gremios registrados",
				Language.Ru => "Нет зарегистрированных гильдий",
				Language.It => "Non ci sono gilde registrate",
				_ => "There are no guilds registered"
			},
			ApiError.InvalidAuditLogEvent => PreferredLanguage switch
			{
				Language.De => "Ungültiger Auditlogeventstyp",
				Language.Fr => "Type d'événement auditlog non valide",
				Language.Es => "Tipo de evento de auditoría no válido",
				Language.Ru => "Неверный тип auditlogevent",
				Language.It => "Tipo di evento auditlog non valido",
				_ => "Invalid auditlogevent type"
			},
			ApiError.ProtectedScheduledMessage => PreferredLanguage switch
			{
				Language.De => "Die geplante Nachricht ist geschützt und kann nicht gelöscht werden.",
				Language.Fr => "Le message planifié est protégé et ne peut pas être supprimé.",
				Language.Es => "El mensaje programado está protegido y no se puede eliminar.",
				Language.Ru => "Запланированное сообщение защищено и не может быть удалено.",
				Language.It => "Il messaggio programmato è protetto e non può essere eliminato.",
				_ => "The scheduled message is protected and cannot be deleted.",
			},
			ApiError.InvalidDateForScheduledMessage => PreferredLanguage switch
			{
				Language.De => "Das Ausführungsdatum muss mindestens eine Minute in der Zukunft liegen.",
				Language.Fr => "La date d'exécution doit être au moins une minute dans le futur.",
				Language.Es => "La fecha de ejecución debe ser al menos un minuto en el futuro.",
				Language.Ru => "Дата выполнения должна быть не менее одной минуты в будущем.",
				Language.It => "La data di esecuzione deve essere almeno un minuto nel futuro.",
				_ => "The execution date has to be at least one minute in the future.",
			},
			_ => "Unknown"
		};
	}

	public string Enum(Language enumValue)
	{
		return enumValue switch
		{
			Language.En => PreferredLanguage switch
			{
				Language.De => "Englisch",
				Language.Fr => "Anglais",
				Language.Es => "inglés",
				Language.Ru => "английский",
				Language.It => "inglese",
				_ => "English"
			},
			Language.De => PreferredLanguage switch
			{
				Language.De => "Deutsch",
				Language.Fr => "Allemand",
				Language.Es => "alemán",
				Language.Ru => "Немецкий",
				Language.It => "Tedesco",
				_ => "German"
			},
			Language.Fr => PreferredLanguage switch
			{
				Language.De => "Französisch",
				Language.Fr => "français",
				Language.Es => "francés",
				Language.Ru => "французкий язык",
				Language.It => "francese",
				_ => "French"
			},
			Language.Es => PreferredLanguage switch
			{
				Language.De => "Spanisch",
				Language.Fr => "Espagnol",
				Language.Es => "Español",
				Language.Ru => "испанский",
				Language.It => "spagnolo",
				_ => "Spanish"
			},
			Language.It => PreferredLanguage switch
			{
				Language.De => "Italienisch",
				Language.Fr => "italien",
				Language.Es => "italiano",
				Language.Ru => "Итальянский",
				Language.It => "italiano",
				_ => "Italian"
			},
			Language.Ru => PreferredLanguage switch
			{
				Language.De => "Russisch",
				Language.Fr => "Russe",
				Language.Es => "Ruso",
				Language.Ru => "Русский",
				Language.It => "Russo",
				_ => "Russian"
			},
			_ => "Unknown"
		};
	}

	public string Enum(ViewPermission enumValue)
	{
		return enumValue switch
		{
			ViewPermission.Self => PreferredLanguage switch
			{
				Language.De => "Privat",
				Language.Fr => "Soi",
				Language.Es => "Uno mismo",
				Language.Ru => "Себя",
				Language.It => "Se stesso",
				_ => "Self"
			},
			ViewPermission.Guild => PreferredLanguage switch
			{
				Language.De => "Gilde",
				Language.Fr => "Guilde",
				Language.Es => "Gremio",
				Language.Ru => "Гильдия",
				Language.It => "Gilda",
				_ => "Guild"
			},
			ViewPermission.Global => PreferredLanguage switch
			{
				Language.De => "Global",
				Language.Fr => "Global",
				Language.Es => "Global",
				Language.Ru => "Глобальный",
				Language.It => "Globale",
				_ => "Global"
			},
			_ => "Unknown"
		};
	}

	public string Enum(EditStatus enumValue)
	{
		return enumValue switch
		{
			EditStatus.None => PreferredLanguage switch
			{
				Language.De => "Unbestimmt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			EditStatus.Unedited => PreferredLanguage switch
			{
				Language.De => "Nicht bearbeitet",
				Language.Fr => "Non édité",
				Language.Es => "No editado",
				Language.Ru => "Не редактировалось",
				Language.It => "Non modificato",
				_ => "Not edited"
			},
			EditStatus.Edited => PreferredLanguage switch
			{
				Language.De => "Bearbeitet",
				Language.Fr => "Édité",
				Language.Es => "Editado",
				Language.Ru => "Отредактировано",
				Language.It => "Modificato",
				_ => "Edited"
			},
			_ => "Unknown"
		};
	}
}