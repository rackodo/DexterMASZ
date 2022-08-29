using Bot.Abstractions;
using Bot.Enums;

namespace GuildAudits.Translators;

public class GuildAuditTranslator : Translator
{
	public string Created()
	{
		return PreferredLanguage switch
		{
			Language.De => "Erstellt",
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
			Language.Fr => "Informations non mises en cache.",
			Language.Es => "Información no almacenada en caché.",
			Language.Ru => "Информация не кешируется.",
			Language.It => "Informazioni non memorizzate nella cache.",
			_ => "Information not cached."
		};
	}

	public string ChannelBefore()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kanal vorher",
			Language.Fr => "Canaliser avant",
			Language.Es => "Canal antes",
			Language.Ru => "Канал до",
			Language.It => "Canale prima",
			_ => "Channel before",
		};
	}
	public string ChannelAfter()
	{
		return PreferredLanguage switch
		{
			Language.De => "Kanal nachher",
			Language.Fr => "Canaliser après",
			Language.Es => "Canal después",
			Language.Ru => "Канал после",
			Language.It => "Canale dopo",
			_ => "Channel after",
		};
	}

	public string Emote()
	{
		return PreferredLanguage switch
		{
			 Language.De => "Emote",
			 Language.Fr => "Émoticône",
			 Language.Es => "Emoción",
			 Language.Ru => "Эмоция",
			 Language.It => "Emote",
			_ => "Emote",
		};
	}

	public string Pinned()
	{
		return PreferredLanguage switch
		{
			Language.De => "Gepinnt",
			Language.Fr => "Épinglé",
			Language.Es => "Fijado",
			Language.Ru => "Закрепленный",
			Language.It => "In evidenza",
			_ => "Pinned",
		};
	}

	public string ReactionAdded()
	{
		return PreferredLanguage switch
		{
			Language.De => "Reaktion hinzugefügt",
			Language.Fr => "Réaction ajoutée",
			Language.Es => "Reacción añadida",
			Language.Ru => "Реакция добавлена",
			Language.It => "Risposta aggiunta",
			_ => "Reaction added",
		};
	}

	public string ReactionRemoved()
	{
		return PreferredLanguage switch
		{
			Language.De => "Reaktion entfernt",
			Language.Fr => "Réaction supprimée",
			Language.Es => "Reacción eliminada",
			Language.Ru => "Реакция удалена",
			Language.It => "Risposta rimossa",
			_ => "Reaction removed",
		};
	}

	public string VoiceJoined()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sprachkanal betreten",
			Language.Fr => "Canal vocal rejoint",
			Language.Es => "Canal de voz unido",
			Language.Ru => "Голосовой канал присоединился",
			Language.It => "Canale vocale unito",
			_ => "Voice channel joined",
		};
	}

	public string VoiceLeft()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sprachkanal verlassen",
			Language.Fr => "Canal vocal quitté",
			Language.Es => "Canal de voz abandonado",
			Language.Ru => "Голосовой канал покинул",
			Language.It => "Canale vocale lasciato",
			_ => "Voice channel left",
		};
	}

	public string MovedVoiceChannel()
	{
		return PreferredLanguage switch
		{
			Language.De => "Sprachkanal gewechselt",
			Language.Fr => "Canal vocal changé",
			Language.Es => "Canal de voz cambiado",
			Language.Ru => "Переключен голосовой канал",
			Language.It => "Canale vocale cambiato",
			_ => "Switched voice channel",
		};
	}

	public string Old()
	{
		return PreferredLanguage switch
		{
			Language.De => "Alt",
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
			Language.De => "Nachricht Gesendet",
			Language.Fr => "Message Envoyé",
			Language.Es => "Mensaje Enviado",
			Language.Ru => "Сообщение отправлено",
			Language.It => "Messaggio Inviato",
			_ => "Message Sent"
		};
	}

	public string MessageUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht Aktualisiert",
			Language.Fr => "Message Modifié",
			Language.Es => "Mensaje Editado",
			Language.Ru => "Сообщение отредактировано",
			Language.It => "Messaggio Modificato",
			_ => "Message Edited"
		};
	}

	public string Before()
	{
		return PreferredLanguage switch
		{
			Language.De => "Zuvor",
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
			Language.Fr => "Message supprimé",
			Language.Es => "Mensaje borrado",
			Language.Ru => "Сообщение удалено",
			Language.It => "Messaggio cancellato",
			_ => "Message Deleted"
		};
	}

	public string Content()
	{
		return PreferredLanguage switch
		{
			Language.De => "Inhalt",
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
			Language.De => "Nutzer Gebannt",
			Language.Fr => "Utilisateur Banni",
			Language.Es => "Usuario Baneado",
			Language.Ru => "Пользователь забанен",
			Language.It => "Utente Bannato",
			_ => "User Banned"
		};
	}

	public string UserUnbanned()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nutzer Entbannt",
			Language.Fr => "Utilisateur Non Banni",
			Language.Es => "Usuario No Prohibido",
			Language.Ru => "Пользователь разблокирован",
			Language.It => "Utente Non Bannato",
			_ => "User Unbanned"
		};
	}

	public string InviteCreated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Einladung Erstellt",
			Language.Fr => "Invitation Créée",
			Language.Es => "Invitación Creada",
			Language.Ru => "Приглашение создано",
			Language.It => "Invito Creato",
			_ => "Invite Created"
		};
	}

	public string Url()
	{
		return PreferredLanguage switch
		{
			Language.De => "URL",
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
			Language.De => "Einladung Gelöscht",
			Language.Fr => "Invitation Supprimée",
			Language.Es => "Invitación Eliminada",
			Language.Ru => "Приглашение удалено",
			Language.It => "Invito Cancellato",
			_ => "Invite Deleted"
		};
	}

	public string UserJoined()
	{
		return PreferredLanguage switch
		{
			Language.De => "Mitglied Beigetreten",
			Language.Fr => "Membre Rejoint",
			Language.Es => "Miembro Se Unió",
			Language.Ru => "Участник присоединился",
			Language.It => "Membro Iscritto",
			_ => "User Joined"
		};
	}

	public string Registered()
	{
		return PreferredLanguage switch
		{
			Language.De => "Registriert",
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
			Language.De => "Mitglied Entfernt",
			Language.Fr => "Membre Supprimé",
			Language.Es => "Miembro Eliminado",
			Language.Ru => "Участник удален",
			Language.It => "Membro Rimosso",
			_ => "User Removed"
		};
	}

	public string ThreadCreated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Thread Erstellt",
			Language.Fr => "Fil Créé",
			Language.Es => "Hilo Creado",
			Language.Ru => "Тема создана",
			Language.It => "Discussione Creata",
			_ => "Thread Created"
		};
	}

	public string Parent()
	{
		return PreferredLanguage switch
		{
			Language.De => "Elternkanal",
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
			Language.De => "Benutzername Aktualisiert",
			Language.Fr => "Nom D'utilisateur Mis à Jour",
			Language.Es => "Nombre de Usuario Actualizado",
			Language.Ru => "Имя пользователя обновлено",
			Language.It => "Nome Utente Aggiornato",
			_ => "Username Updated"
		};
	}

	public string AvatarUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Avatar Aktualisiert",
			Language.Fr => "Avatar Mis à Jour",
			Language.Es => "Avatar Actualizado",
			Language.Ru => "Аватар обновлен",
			Language.It => "Avatar Aggiornato",
			_ => "Avatar Updated"
		};
	}

	public string NicknameUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nickname Aktualisiert",
			Language.Fr => "Pseudo Mis à Jour",
			Language.Es => "Se Actualizó el Apodo",
			Language.Ru => "Псевдоним обновлен",
			Language.It => "Nickname Aggiornato",
			_ => "Nickname Updated"
		};
	}

	public string RolesUpdated()
	{
		return PreferredLanguage switch
		{
			Language.De => "Rollen Aktualisiert",
			Language.Fr => "Rôles Mis à Jour",
			Language.Es => "Funciones Actualizadas",
			Language.Ru => "Роли обновлены",
			Language.It => "Ruoli Aggiornati",
			_ => "Roles Updated"
		};
	}

	public string Added()
	{
		return PreferredLanguage switch
		{
			Language.De => "Hinzugefügt",
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
			Language.Fr => "Supprimé",
			Language.Es => "Remoto",
			Language.Ru => "Удаленный",
			Language.It => "RIMOSSO",
			_ => "Removed"
		};
	}
}