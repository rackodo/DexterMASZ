using Bot.Abstractions;
using Bot.Enums;
using GuildAudits.Enums;

namespace GuildAudits.Translators;

public class GuildAuditEnumTranslator : Translator
{
	public string Enum(GuildAuditEvent enumValue)
	{
		return enumValue switch
		{
			GuildAuditEvent.MessageSent => PreferredLanguage switch
			{
				Language.De => "Nachricht gesendet",
				Language.At => "Nochricht gsendet",
				Language.Fr => "Message envoyé",
				Language.Es => "Mensaje enviado",
				Language.Ru => "Сообщение отправлено",
				Language.It => "Messaggio inviato",
				_ => "Message sent"
			},
			GuildAuditEvent.MessageUpdated => PreferredLanguage switch
			{
				Language.De => "Nachricht aktualisiert",
				Language.At => "Nochricht aktualisiat",
				Language.Fr => "Message mis à jour",
				Language.Es => "Mensaje actualizado",
				Language.Ru => "Сообщение обновлено",
				Language.It => "Messaggio aggiornato",
				_ => "Message updated"
			},
			GuildAuditEvent.MessageDeleted => PreferredLanguage switch
			{
				Language.De => "Nachricht gelöscht",
				Language.At => "Nochricht glescht",
				Language.Fr => "Message supprimé",
				Language.Es => "Mensaje borrado",
				Language.Ru => "Сообщение удалено",
				Language.It => "Messaggio cancellato",
				_ => "Message deleted"
			},
			GuildAuditEvent.UsernameUpdated => PreferredLanguage switch
			{
				Language.De => "Benutzername aktualisiert",
				Language.At => "Benutzaname aktualisiat",
				Language.Fr => "Nom d'utilisateur mis à jour",
				Language.Es => "Nombre de usuario actualizado",
				Language.Ru => "Имя пользователя обновлено",
				Language.It => "Nome utente aggiornato",
				_ => "Username updated"
			},
			GuildAuditEvent.AvatarUpdated => PreferredLanguage switch
			{
				Language.De => "Avatar aktualisiert",
				Language.At => "Avatar aktualisiat",
				Language.Fr => "Avatar mis à jour",
				Language.Es => "Avatar actualizado",
				Language.Ru => "Аватар обновлен",
				Language.It => "Avatar aggiornato",
				_ => "Avatar updated"
			},
			GuildAuditEvent.NicknameUpdated => PreferredLanguage switch
			{
				Language.De => "Nickname aktualisiert",
				Language.At => "Nickname aktualisiat",
				Language.Fr => "Pseudo mis à jour",
				Language.Es => "Se actualizó el apodo",
				Language.Ru => "Псевдоним обновлен",
				Language.It => "Nickname aggiornato",
				_ => "Nickname updated"
			},
			GuildAuditEvent.UserRolesUpdated => PreferredLanguage switch
			{
				Language.De => "Mitgliederrollen aktualisiert",
				Language.At => "Mitgliedarollen aktualisiat",
				Language.Fr => "Rôles des membres mis à jour",
				Language.Es => "Se actualizaron las funciones de los miembros",
				Language.Ru => "Роли участников обновлены",
				Language.It => "Ruoli dei membri aggiornati",
				_ => "User roles updated"
			},
			GuildAuditEvent.UserJoined => PreferredLanguage switch
			{
				Language.De => "Mitglied beigetreten",
				Language.At => "Mitglied beitretn",
				Language.Fr => "Membre rejoint",
				Language.Es => "Miembro se unió",
				Language.Ru => "Участник присоединился",
				Language.It => "Membro iscritto",
				_ => "User joined"
			},
			GuildAuditEvent.UserRemoved => PreferredLanguage switch
			{
				Language.De => "Mitglied entfernt",
				Language.At => "Mitglied entfernt",
				Language.Fr => "Membre supprimé",
				Language.Es => "Miembro eliminado",
				Language.Ru => "Участник удален",
				Language.It => "Membro rimosso",
				_ => "User removed"
			},
			GuildAuditEvent.BanAdded => PreferredLanguage switch
			{
				Language.De => "Mitglied gebannt",
				Language.At => "Mitglied ausgsperrt",
				Language.Fr => "Membre banni",
				Language.Es => "Miembro prohibido",
				Language.Ru => "Участник забанен",
				Language.It => "Membro bannato",
				_ => "User banned"
			},
			GuildAuditEvent.BanRemoved => PreferredLanguage switch
			{
				Language.De => "Mitglied entsperrt",
				Language.At => "Mitglied nimma ausgsperrt",
				Language.Fr => "Membre non banni",
				Language.Es => "Miembro no prohibido",
				Language.Ru => "Участник разблокирован",
				Language.It => "Membro non bannato",
				_ => "User unbanned"
			},
			GuildAuditEvent.InviteCreated => PreferredLanguage switch
			{
				Language.De => "Einladung erstellt",
				Language.At => "Eiladung erstöt",
				Language.Fr => "Invitation créée",
				Language.Es => "Invitación creada",
				Language.Ru => "Приглашение создано",
				Language.It => "Invito creato",
				_ => "Invite created"
			},
			GuildAuditEvent.InviteDeleted => PreferredLanguage switch
			{
				Language.De => "Einladung gelöscht",
				Language.At => "Einladung glescht",
				Language.Fr => "Invitation supprimée",
				Language.Es => "Invitación eliminada",
				Language.Ru => "Приглашение удалено",
				Language.It => "Invito cancellato",
				_ => "Invite deleted"
			},
			GuildAuditEvent.ThreadCreated => PreferredLanguage switch
			{
				Language.De => "Thema erstellt",
				Language.At => "Fadn erstöt",
				Language.Fr => "Fil créé",
				Language.Es => "Hilo creado",
				Language.Ru => "Тема создана",
				Language.It => "Discussione creata",
				_ => "Thread created"
			},
			_ => "Unknown"
		};
	}
}