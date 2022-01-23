using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.GuildAudits.Translators;

public class GuildAuditTranslator : Translator
{
	public string Created()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erstellt",
			Language.At => "Erstöt",
			Language.Fr => "Créé",
			Language.Es => "Creado",
			Language.Ru => "Созданный",
			Language.It => "Creato",
			_ => "Created"
		};
	}
	
	public string InformationNotCached()
	{
		return PreferredLanguage switch
		{
			Language.De => "Information nicht im Cache.",
			Language.At => "Info ned im Cache.",
			Language.Fr => "Informations non mises en cache.",
			Language.Es => "Información no almacenada en caché.",
			Language.Ru => "Информация не кешируется.",
			Language.It => "Informazioni non memorizzate nella cache.",
			_ => "Information not cached."
		};
	}

	public string Old()
	{
		return PreferredLanguage switch
		{
			Language.De => "Alt",
			Language.At => "Alt",
			Language.Fr => "Ancien",
			Language.Es => "Viejo",
			Language.Ru => "Старый",
			Language.It => "Vecchio",
			_ => "Old"
		};
	}

	public string New()
	{
		return PreferredLanguage switch
		{
			Language.De => "Neu",
			Language.At => "Neu",
			Language.Fr => "Nouveau",
			Language.Es => "Nuevo",
			Language.Ru => "Новый",
			Language.It => "Nuovo",
			_ => "New"
		};
	}

	public string Empty()
	{
		return PreferredLanguage switch
		{
			Language.De => "Leer",
			Language.At => "Leer",
			Language.Fr => "Vide",
			Language.Es => "Vacío",
			Language.Ru => "Пустой",
			Language.It => "Vuoto",
			_ => "Empty"
		};
	}

	public string MessageSent()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht gesendet",
			Language.At => "Nochricht gsendet",
			Language.Fr => "Message envoyé",
			Language.Es => "Mensaje enviado",
			Language.Ru => "Сообщение отправлено",
			Language.It => "Messaggio inviato",
			_ => "Message sent"
		};
	}

	public string MessageUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht aktualisiert",
			Language.At => "Nochricht aktualisiert",
			Language.Fr => "Message modifié",
			Language.Es => "Mensaje editado",
			Language.Ru => "Сообщение отредактировано",
			Language.It => "Messaggio modificato",
			_ => "Message edited"
		};
	}

	public string Before()
	{
		return PreferredLanguage switch
		{
			Language.De => "Zuvor",
			Language.At => "Davoa",
			Language.Fr => "Avant",
			Language.Es => "Antes",
			Language.Ru => "До",
			Language.It => "Prima",
			_ => "Before"
		};
	}

	public string MessageDeleted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht gelöscht",
			Language.At => "Nochricht gelöscht",
			Language.Fr => "Message supprimé",
			Language.Es => "Mensaje borrado",
			Language.Ru => "Сообщение удалено",
			Language.It => "Messaggio cancellato",
			_ => "Message deleted"
		};
	}

	public string Content()
	{
		return PreferredLanguage switch
		{
			Language.De => "Inhalt",
			Language.At => "Inhalt",
			Language.Fr => "Teneur",
			Language.Es => "Contenido",
			Language.Ru => "Содержание",
			Language.It => "Contenuto",
			_ => "Content"
		};
	}

	public string UserBanned()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nutzer gebannt",
			Language.At => "Nutza ausgsperrt",
			Language.Fr => "Utilisateur banni",
			Language.Es => "Usuario baneado",
			Language.Ru => "Пользователь забанен",
			Language.It => "Utente bannato",
			_ => "User banned"
		};
	}

	public string UserUnbanned()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nutzer entbannt",
			Language.At => "Nutza nimma ausgsperrt",
			Language.Fr => "Utilisateur non banni",
			Language.Es => "Usuario no prohibido",
			Language.Ru => "Пользователь разблокирован",
			Language.It => "Utente non bannato",
			_ => "User unbanned"
		};
	}

	public string InviteCreated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Einladung erstellt",
			Language.At => "Eiladung erstöt",
			Language.Fr => "Invitation créée",
			Language.Es => "Invitación creada",
			Language.Ru => "Приглашение создано",
			Language.It => "Invito creato",
			_ => "Invite created"
		};
	}

	public string Url()
	{
		return PreferredLanguage switch
		{
			Language.De => "URL",
			Language.At => "URL",
			Language.Fr => "URL",
			Language.Es => "URL",
			Language.Ru => "URL",
			Language.It => "URL",
			_ => "URL"
		};
	}

	public string MaxUses()
	{
		return PreferredLanguage switch
		{
			Language.De => "Maximale Nutzungen",
			Language.At => "Maximale Vawednungen",
			Language.Fr => "Utilisations maximales",
			Language.Es => "Usos máximos",
			Language.Ru => "Макс использует",
			Language.It => "Usi massimi",
			_ => "Max uses"
		};
	}

	public string ExpirationDate()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ablaufdatum",
			Language.At => "Oblaufdatum",
			Language.Fr => "Date d'expiration",
			Language.Es => "Fecha de caducidad",
			Language.Ru => "Срок хранения",
			Language.It => "Data di scadenza",
			_ => "Expiration date"
		};
	}

	public string TargetChannel()
	{
		return PreferredLanguage switch
		{
			Language.De => "Zielkanal",
			Language.At => "Zielkanal",
			Language.Fr => "Canal cible",
			Language.Es => "Canal objetivo",
			Language.Ru => "Целевой канал",
			Language.It => "Canale di destinazione",
			_ => "Target channel"
		};
	}

	public string InviteDeleted()
	{
		return PreferredLanguage switch
		{
			Language.De => "Einladung gelöscht",
			Language.At => "Eiladung glescht",
			Language.Fr => "Invitation supprimée",
			Language.Es => "Invitación eliminada",
			Language.Ru => "Приглашение удалено",
			Language.It => "Invito cancellato",
			_ => "Invite deleted"
		};
	}

	public string UserJoined()
	{
		return PreferredLanguage switch
		{
			Language.De => "Mitglied beigetreten",
			Language.At => "Mitglied beitretn",
			Language.Fr => "Membre rejoint",
			Language.Es => "Miembro se unió",
			Language.Ru => "Участник присоединился",
			Language.It => "Membro iscritto",
			_ => "User joined"
		};
	}

	public string Registered()
	{
		return PreferredLanguage switch
		{
			Language.De => "Registriert",
			Language.At => "Registriat",
			Language.Fr => "Inscrit",
			Language.Es => "Registrado",
			Language.Ru => "Зарегистрировано",
			Language.It => "Registrato",
			_ => "Registered"
		};
	}

	public string UserRemoved()
	{
		return PreferredLanguage switch
		{
			Language.De => "Mitglied entfernt",
			Language.At => "Mitglied entfernt",
			Language.Fr => "Membre supprimé",
			Language.Es => "Miembro eliminado",
			Language.Ru => "Участник удален",
			Language.It => "Membro rimosso",
			_ => "User removed"
		};
	}

	public string ThreadCreated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Thread erstellt",
			Language.At => "Thread erstöt",
			Language.Fr => "Fil créé",
			Language.Es => "Hilo creado",
			Language.Ru => "Тема создана",
			Language.It => "Discussione creata",
			_ => "Thread created"
		};
	}

	public string Parent()
	{
		return PreferredLanguage switch
		{
			Language.De => "Elternkanal",
			Language.At => "Eltankanal",
			Language.Fr => "Parent",
			Language.Es => "Padre",
			Language.Ru => "Родитель",
			Language.It => "Genitore",
			_ => "Parent"
		};
	}

	public string Creator()
	{
		return PreferredLanguage switch
		{
			Language.De => "Ersteller",
			Language.At => "Erstölla",
			Language.Fr => "Créateur",
			Language.Es => "Creador",
			Language.Ru => "Создатель",
			Language.It => "Creatore",
			_ => "Creator"
		};
	}

	public string UsernameUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzername aktualisiert",
			Language.At => "Benutzanom aktualisiat",
			Language.Fr => "Nom d'utilisateur mis à jour",
			Language.Es => "Nombre de usuario actualizado",
			Language.Ru => "Имя пользователя обновлено",
			Language.It => "Nome utente aggiornato",
			_ => "Username updated"
		};
	}

	public string AvatarUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Avatar aktualisiert",
			Language.At => "Avatar aktualisiat",
			Language.Fr => "Avatar mis à jour",
			Language.Es => "Avatar actualizado",
			Language.Ru => "Аватар обновлен",
			Language.It => "Avatar aggiornato",
			_ => "Avatar updated"
		};
	}

	public string NicknameUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nickname aktualisiert",
			Language.At => "Nickname aktualisiat",
			Language.Fr => "Pseudo mis à jour",
			Language.Es => "Se actualizó el apodo",
			Language.Ru => "Псевдоним обновлен",
			Language.It => "Nickname aggiornato",
			_ => "Nickname updated"
		};
	}

	public string RolesUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Rollen aktualisiert",
			Language.At => "Rollen aktualisiat",
			Language.Fr => "Rôles mis à jour",
			Language.Es => "Funciones actualizadas",
			Language.Ru => "Роли обновлены",
			Language.It => "Ruoli aggiornati",
			_ => "Roles updated"
		};
	}

	public string Added()
	{
		return PreferredLanguage switch
		{
			Language.De => "Hinzugefügt",
			Language.At => "Hinzugfügt",
			Language.Fr => "Ajoutée",
			Language.Es => "Adicional",
			Language.Ru => "Добавлен",
			Language.It => "Aggiunto",
			_ => "Added"
		};
	}

	public string Removed()
	{
		return PreferredLanguage switch
		{
			Language.De => "Entfernt",
			Language.At => "Entfernt",
			Language.Fr => "Supprimé",
			Language.Es => "Remoto",
			Language.Ru => "Удаленный",
			Language.It => "RIMOSSO",
			_ => "Removed"
		};
	}
}