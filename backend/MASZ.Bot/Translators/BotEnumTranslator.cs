using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Bot.Translators;

public class BotEnumTranslator : Translator
{
	public string Enum(ApiError enumValue)
	{
		return enumValue switch
		{
			ApiError.Unknown => PreferredLanguage switch
			{
				Language.De => "Unbekannter Fehler",
				Language.At => "Unbekonnta Föhla",
				Language.Fr => "Erreur inconnue",
				Language.Es => "Error desconocido",
				Language.Ru => "Неизвестная ошибка",
				Language.It => "Errore sconosciuto",
				_ => "Unknown error"
			},
			ApiError.InvalidDiscordUser => PreferredLanguage switch
			{
				Language.De => "Ungültiger Discordbenutzer",
				Language.At => "Ungütiga Discordbenutza",
				Language.Fr => "Utilisateur discord invalide",
				Language.Es => "Usuario de discordia no válido",
				Language.Ru => "Недействительный пользователь Discord",
				Language.It => "Utente discord non valido",
				_ => "Invalid discord user"
			},
			ApiError.ProtectedModCaseSuspect => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt",
				Language.At => "Benutza is gschützt",
				Language.Fr => "L'utilisateur est protégé",
				Language.Es => "El usuario está protegido",
				Language.Ru => "Пользователь защищен",
				Language.It => "L'utente è protetto",
				_ => "User is protected"
			},
			ApiError.ProtectedModCaseSuspectIsBot => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Bot.",
				Language.At => "Benutza is gschützt, es is a Bot.",
				Language.Fr => "L'utilisateur est protégé. C'est un robot.",
				Language.Es => "El usuario está protegido. El es un bot.",
				Language.Ru => "Пользователь защищен. Он бот.",
				Language.It => "L'utente è protetto. Lui è un bot.",
				_ => "User is protected. He is a bot."
			},
			ApiError.ProtectedModCaseSuspectIsSiteAdmin => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Seitenadministrator.",
				Language.At => "Benutza is gschützt, er is a Seitenadministraotr.",
				Language.Fr => "L'utilisateur est protégé. Il est administrateur du site.",
				Language.Es => "El usuario está protegido. Es administrador de un sitio.",
				Language.Ru => "Пользователь защищен. Он администратор сайта.",
				Language.It => "L'utente è protetto. È un amministratore del sito.",
				_ => "User is protected. He is a site admin."
			},
			ApiError.ProtectedModCaseSuspectIsTeam => PreferredLanguage switch
			{
				Language.De => "Benutzer ist geschützt. Er ist ein Teammitglied.",
				Language.At => "Benutza is gschützt, er is a Teammitglied.",
				Language.Fr => "L'utilisateur est protégé. Il est membre de l'équipe.",
				Language.Es => "El usuario está protegido. Es un miembro del equipo.",
				Language.Ru => "Пользователь защищен. Он член команды.",
				Language.It => "L'utente è protetto. È un membro della squadra.",
				_ => "User is protected. He is a team user."
			},
			ApiError.ResourceNotFound => PreferredLanguage switch
			{
				Language.De => "Ressource nicht gefunden",
				Language.At => "Ressource ned gfundn.",
				Language.Fr => "Ressource introuvable",
				Language.Es => "Recurso no encontrado",
				Language.Ru => "Ресурс не найден",
				Language.It => "Risorsa non trovata",
				_ => "Resource not found"
			},
			ApiError.InvalidIdentity => PreferredLanguage switch
			{
				Language.De => "Ungültige Identität",
				Language.At => "Ungültige Identität",
				Language.Fr => "Identité invalide",
				Language.Es => "Identidad inválida",
				Language.Ru => "Неверная личность",
				Language.It => "Identità non valida",
				_ => "Invalid identity"
			},
			ApiError.GuildUnregistered => PreferredLanguage switch
			{
				Language.De => "Gilde ist nicht registriert",
				Language.At => "Güde is ned registriat",
				Language.Fr => "La guilde n'est pas enregistrée",
				Language.Es => "El gremio no está registrado",
				Language.Ru => "Гильдия не зарегистрирована",
				Language.It => "La gilda non è registrata",
				_ => "Guild is not registered"
			},
			ApiError.Unauthorized => PreferredLanguage switch
			{
				Language.De => "Nicht berechtigt",
				Language.At => "Ned berechtigt",
				Language.Fr => "Non autorisé",
				Language.Es => "No autorizado",
				Language.Ru => "Неавторизованный",
				Language.It => "non autorizzato",
				_ => "Unauthorized"
			},
			ApiError.ModCaseIsMarkedToBeDeleted => PreferredLanguage switch
			{
				Language.De => "ModCase ist zum Löschen markiert",
				Language.At => "ModCase is zum Löscha markiat",
				Language.Fr => "ModCase est marqué pour être supprimé",
				Language.Es => "ModCase está marcado para ser eliminado",
				Language.Ru => "ModCase отмечен для удаления",
				Language.It => "ModCase è contrassegnato per essere eliminato",
				_ => "ModCase is marked to be deleted"
			},
			ApiError.ModCaseIsNotMarkedToBeDeleted => PreferredLanguage switch
			{
				Language.De => "ModCase ist nicht zum Löschen markiert",
				Language.At => "ModCase is ned zum Lösche markiat",
				Language.Fr => "ModCase n'est pas marqué pour être supprimé",
				Language.Es => "ModCase no está marcado para ser eliminado",
				Language.Ru => "ModCase не отмечен для удаления",
				Language.It => "ModCase non è contrassegnato per essere eliminato",
				_ => "ModCase is not marked to be deleted"
			},
			ApiError.GuildAlreadyRegistered => PreferredLanguage switch
			{
				Language.De => "Gilde ist bereits registriert",
				Language.At => "Güde is bereits registriat",
				Language.Fr => "La guilde est déjà enregistrée",
				Language.Es => "El gremio ya está registrado",
				Language.Ru => "Гильдия уже зарегистрирована",
				Language.It => "La gilda è già registrata",
				_ => "Guild is already registered"
			},
			ApiError.NotAllowedInDemoMode => PreferredLanguage switch
			{
				Language.De => "Diese Aktion ist in der Demo-Version nicht erlaubt",
				Language.At => "De Aktion is in da Demo-Version ned erlaubt",
				Language.Fr => "Cette action n'est pas autorisée en mode démo",
				Language.Es => "Esta acción no está permitida en el modo de demostración.",
				Language.Ru => "Это действие запрещено в демонстрационном режиме.",
				Language.It => "Questa azione non è consentita in modalità demo",
				_ => "This action is not allowed in demo mode"
			},
			ApiError.RoleNotFound => PreferredLanguage switch
			{
				Language.De => "Rolle nicht gefunden",
				Language.At => "Rolle ned gfundn",
				Language.Fr => "Rôle introuvable",
				Language.Es => "Rol no encontrado",
				Language.Ru => "Роль не найдена",
				Language.It => "Ruolo non trovato",
				_ => "Role not found"
			},
			ApiError.TokenCannotManageThisResource => PreferredLanguage switch
			{
				Language.De => "Tokens können diese Ressource nicht verwalten",
				Language.At => "Tokns kennan de Ressourcen ned vawoitn",
				Language.Fr => "Les jetons ne peuvent pas gérer cette ressource",
				Language.Es => "Los tokens no pueden administrar este recurso",
				Language.Ru => "Лексемы не могут управлять этим ресурсом",
				Language.It => "I token non possono gestire questa risorsa",
				_ => "Tokens cannot manage this resource"
			},
			ApiError.TokenAlreadyRegistered => PreferredLanguage switch
			{
				Language.De => "Token ist bereits registriert",
				Language.At => "Tokn is bereits registriat",
				Language.Fr => "Le jeton est déjà enregistré",
				Language.Es => "El token ya está registrado",
				Language.Ru => "Токен уже зарегистрирован",
				Language.It => "Il token è già registrato",
				_ => "Token is already registered"
			},
			ApiError.CannotBeSameUser => PreferredLanguage switch
			{
				Language.De => "Beide Benutzer sind gleich.",
				Language.At => "Beide Benutza san gleich.",
				Language.Fr => "Les deux utilisateurs sont les mêmes.",
				Language.Es => "Ambos usuarios son iguales.",
				Language.Ru => "Оба пользователя одинаковые.",
				Language.It => "Entrambi gli utenti sono gli stessi.",
				_ => "Both users are the same."
			},
			ApiError.ResourceAlreadyExists => PreferredLanguage switch
			{
				Language.De => "Ressource existiert bereits",
				Language.At => "De Ressource gibts bereits",
				Language.Fr => "La ressource existe déjà",
				Language.Es => "El recurso ya existe",
				Language.Ru => "Ресурс уже существует",
				Language.It => "La risorsa esiste già",
				_ => "Resource already exists"
			},
			ApiError.ModCaseDoesNotAllowComments => PreferredLanguage switch
			{
				Language.De => "Kommentare sind für diesen Vorfall gesperrt",
				Language.At => "Kommentare san fia den Vorfoi gsperrt",
				Language.Fr => "Les commentaires sont verrouillés pour ce modcase",
				Language.Es => "Los comentarios están bloqueados para este modcase",
				Language.Ru => "Комментарии заблокированы для этого мода",
				Language.It => "I commenti sono bloccati per questo modcase",
				_ => "Comments are locked for this modcase"
			},
			ApiError.LastCommentAlreadyFromSuspect => PreferredLanguage switch
			{
				Language.De => "Der letzte Kommentar war schon von dem Beschuldigten.",
				Language.At => "Da letzte Kommentar woa scho vom Beschuldigten.",
				Language.Fr => "Le dernier commentaire était déjà du suspect.",
				Language.Es => "El último comentario ya era del sospechoso.",
				Language.Ru => "Последний комментарий уже был от подозреваемого.",
				Language.It => "L'ultimo commento era già del sospettato.",
				_ => "The last comment was already from the suspect."
			},
			ApiError.InvalidAutoModerationAction => PreferredLanguage switch
			{
				Language.De => "Ungültige AutoModerationsaktion",
				Language.At => "Ned gütige automodarationsaktion",
				Language.Fr => "Action de modération automatique non valide",
				Language.Es => "Acción de automoderación no válida",
				Language.Ru => "Недопустимое действие автомодерации",
				Language.It => "Azione di moderazione automatica non valida",
				_ => "Invalid AutoModeration action"
			},
			ApiError.InvalidAutoModerationType => PreferredLanguage switch
			{
				Language.De => "Ungültiger AutoModerationstyp",
				Language.At => "Ungütiga automodarationstyp",
				Language.Fr => "Type d'automodération non valide",
				Language.Es => "Tipo de automoderación no válido",
				Language.Ru => "Неверный тип автомодерации.",
				Language.It => "Tipo di moderazione automatica non valido",
				_ => "Invalid AutoModeration type"
			},
			ApiError.TooManyTemplates => PreferredLanguage switch
			{
				Language.De => "Benutzer hat die maximale Anzahl an Templates erreicht",
				Language.At => "Benutza hod de maximale Onzoi vo de Templates erreicht",
				Language.Fr => "L'utilisateur a atteint la limite maximale de modèles",
				Language.Es => "El usuario ha alcanzado el límite máximo de plantillas",
				Language.Ru => "Пользователь достиг максимального предела шаблонов",
				Language.It => "L'utente ha raggiunto il limite massimo di modelli",
				_ => "User has reached the max limit of templates"
			},
			ApiError.InvalidFilePath => PreferredLanguage switch
			{
				Language.De => "Ungültiger Dateipfad",
				Language.At => "Ungütiga Dateipfad",
				Language.Fr => "Chemin de fichier invalide",
				Language.Es => "Ruta de archivo no válida",
				Language.Ru => "Неверный путь к файлу",
				Language.It => "Percorso file non valido",
				_ => "Invalid file path"
			},
			ApiError.NoGuildsRegistered => PreferredLanguage switch
			{
				Language.De => "Es sind keine Gilden registriert",
				Language.At => "Es san kane Güdn registriat",
				Language.Fr => "Il n'y a pas de guildes enregistrées",
				Language.Es => "No hay gremios registrados",
				Language.Ru => "Нет зарегистрированных гильдий",
				Language.It => "Non ci sono gilde registrate",
				_ => "There are no guilds registered"
			},
			ApiError.InvalidAuditLogEvent => PreferredLanguage switch
			{
				Language.De => "Ungültiger Auditlogeventstyp",
				Language.At => "Ungütiga oduitlogeventstyp",
				Language.Fr => "Type d'événement auditlog non valide",
				Language.Es => "Tipo de evento de auditoría no válido",
				Language.Ru => "Неверный тип auditlogevent",
				Language.It => "Tipo di evento auditlog non valido",
				_ => "Invalid auditlogevent type"
			},
			ApiError.ProtectedScheduledMessage => PreferredLanguage switch
			{
				Language.De => "Die geplante Nachricht ist geschützt und kann nicht gelöscht werden.",
				Language.At => "Dé geplanten Nachricht ist geschützt und kann nicht gelöscht werden.",
				Language.Fr => "Le message planifié est protégé et ne peut pas être supprimé.",
				Language.Es => "El mensaje programado está protegido y no se puede eliminar.",
				Language.Ru => "Запланированное сообщение защищено и не может быть удалено.",
				Language.It => "Il messaggio programmato è protetto e non può essere eliminato.",
				_ => "The scheduled message is protected and cannot be deleted.",
			},
			ApiError.InvalidDateForScheduledMessage => PreferredLanguage switch
			{
				Language.De => "Das Ausführungsdatum muss mindestens eine Minute in der Zukunft liegen.",
				Language.At => "Das Ausführungsdatum muss mindestens eine Minute in der Zukunft liegen.",
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
				Language.At => "Englisch",
				Language.Fr => "Anglais",
				Language.Es => "inglés",
				Language.Ru => "английский",
				Language.It => "inglese",
				_ => "English"
			},
			Language.De => PreferredLanguage switch
			{
				Language.De => "Deutsch",
				Language.At => "Piefchinesisch",
				Language.Fr => "Allemand",
				Language.Es => "alemán",
				Language.Ru => "Немецкий",
				Language.It => "Tedesco",
				_ => "German"
			},
			Language.Fr => PreferredLanguage switch
			{
				Language.De => "Französisch",
				Language.At => "Franzesisch",
				Language.Fr => "français",
				Language.Es => "francés",
				Language.Ru => "французкий язык",
				Language.It => "francese",
				_ => "French"
			},
			Language.Es => PreferredLanguage switch
			{
				Language.De => "Spanisch",
				Language.At => "Spanisch",
				Language.Fr => "Espagnol",
				Language.Es => "Español",
				Language.Ru => "испанский",
				Language.It => "spagnolo",
				_ => "Spanish"
			},
			Language.It => PreferredLanguage switch
			{
				Language.De => "Italienisch",
				Language.At => "Italienisch",
				Language.Fr => "italien",
				Language.Es => "italiano",
				Language.Ru => "Итальянский",
				Language.It => "italiano",
				_ => "Italian"
			},
			Language.At => PreferredLanguage switch
			{
				Language.De => "Österreich",
				Language.At => "Esterreichisch",
				Language.Fr => "autrichien",
				Language.Es => "austriaco",
				Language.Ru => "Австрийский",
				Language.It => "austriaco",
				_ => "Austrian"
			},
			Language.Ru => PreferredLanguage switch
			{
				Language.De => "Russisch",
				Language.At => "Rusisch",
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
				Language.At => "Privot",
				Language.Fr => "Soi",
				Language.Es => "Uno mismo",
				Language.Ru => "Себя",
				Language.It => "Se stesso",
				_ => "Self"
			},
			ViewPermission.Guild => PreferredLanguage switch
			{
				Language.De => "Gilde",
				Language.At => "Güde",
				Language.Fr => "Guilde",
				Language.Es => "Gremio",
				Language.Ru => "Гильдия",
				Language.It => "Gilda",
				_ => "Guild"
			},
			ViewPermission.Global => PreferredLanguage switch
			{
				Language.De => "Global",
				Language.At => "Globoi",
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
				Language.At => "Unbekonnt",
				Language.Fr => "Rien",
				Language.Es => "Ninguna",
				Language.Ru => "Никто",
				Language.It => "Nessuno",
				_ => "None"
			},
			EditStatus.Unedited => PreferredLanguage switch
			{
				Language.De => "Nicht bearbeitet",
				Language.At => "Ned beorbeitet.",
				Language.Fr => "Non édité",
				Language.Es => "No editado",
				Language.Ru => "Не редактировалось",
				Language.It => "Non modificato",
				_ => "Not edited"
			},
			EditStatus.Edited => PreferredLanguage switch
			{
				Language.De => "Bearbeitet",
				Language.At => "Beorbeitet",
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