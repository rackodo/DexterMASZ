using Bot.Abstractions;
using Bot.Enums;
using GuildAudits.Enums;

namespace GuildAudits.Translators;

public class GuildAuditEnumTranslator : Translator
{
	public string Enum(GuildAuditLogEvent enumValue)
	{
		return enumValue switch
		{
			GuildAuditLogEvent.MessageSent => PreferredLanguage switch
			{
				Language.De => "Nachricht gesendet",
				Language.Fr => "Message envoyé",
				Language.Es => "Mensaje enviado",
				Language.Ru => "Сообщение отправлено",
				Language.It => "Messaggio inviato",
				_ => "Message sent"
			},
			GuildAuditLogEvent.MessageUpdated => PreferredLanguage switch
			{
				Language.De => "Nachricht aktualisiert",
				Language.Fr => "Message mis à jour",
				Language.Es => "Mensaje actualizado",
				Language.Ru => "Сообщение обновлено",
				Language.It => "Messaggio aggiornato",
				_ => "Message updated"
			},
			GuildAuditLogEvent.MessageDeleted => PreferredLanguage switch
			{
				Language.De => "Nachricht gelöscht",
				Language.Fr => "Message supprimé",
				Language.Es => "Mensaje borrado",
				Language.Ru => "Сообщение удалено",
				Language.It => "Messaggio cancellato",
				_ => "Message deleted"
			},
			GuildAuditLogEvent.UsernameUpdated => PreferredLanguage switch
			{
				Language.De => "Benutzername aktualisiert",
				Language.Fr => "Nom d'utilisateur mis à jour",
				Language.Es => "Nombre de usuario actualizado",
				Language.Ru => "Имя пользователя обновлено",
				Language.It => "Nome utente aggiornato",
				_ => "Username updated"
			},
			GuildAuditLogEvent.AvatarUpdated => PreferredLanguage switch
			{
				Language.De => "Avatar aktualisiert",
				Language.Fr => "Avatar mis à jour",
				Language.Es => "Avatar actualizado",
				Language.Ru => "Аватар обновлен",
				Language.It => "Avatar aggiornato",
				_ => "Avatar updated"
			},
			GuildAuditLogEvent.NicknameUpdated => PreferredLanguage switch
			{
				Language.De => "Nickname aktualisiert",
				Language.Fr => "Pseudo mis à jour",
				Language.Es => "Se actualizó el apodo",
				Language.Ru => "Псевдоним обновлен",
				Language.It => "Nickname aggiornato",
				_ => "Nickname updated"
			},
			GuildAuditLogEvent.UserRolesUpdated => PreferredLanguage switch
			{
				Language.De => "Mitgliederrollen aktualisiert",
				Language.Fr => "Rôles des membres mis à jour",
				Language.Es => "Se actualizaron las funciones de los miembros",
				Language.Ru => "Роли участников обновлены",
				Language.It => "Ruoli dei membri aggiornati",
				_ => "User roles updated"
			},
			GuildAuditLogEvent.UserJoined => PreferredLanguage switch
			{
				Language.De => "Mitglied beigetreten",
				Language.Fr => "Membre rejoint",
				Language.Es => "Miembro se unió",
				Language.Ru => "Участник присоединился",
				Language.It => "Membro iscritto",
				_ => "User joined"
			},
			GuildAuditLogEvent.UserRemoved => PreferredLanguage switch
			{
				Language.De => "Mitglied entfernt",
				Language.Fr => "Membre supprimé",
				Language.Es => "Miembro eliminado",
				Language.Ru => "Участник удален",
				Language.It => "Membro rimosso",
				_ => "User removed"
			},
			GuildAuditLogEvent.BanAdded => PreferredLanguage switch
			{
				Language.De => "Mitglied gebannt",
				Language.Fr => "Membre banni",
				Language.Es => "Miembro prohibido",
				Language.Ru => "Участник забанен",
				Language.It => "Membro bannato",
				_ => "User banned"
			},
			GuildAuditLogEvent.BanRemoved => PreferredLanguage switch
			{
				Language.De => "Mitglied entsperrt",
				Language.Fr => "Membre non banni",
				Language.Es => "Miembro no prohibido",
				Language.Ru => "Участник разблокирован",
				Language.It => "Membro non bannato",
				_ => "User Unbanned"
			},
			GuildAuditLogEvent.InviteCreated => PreferredLanguage switch
			{
				Language.De => "Einladung erstellt",
				Language.Fr => "Invitation créée",
				Language.Es => "Invitación creada",
				Language.Ru => "Приглашение создано",
				Language.It => "Invito creato",
				_ => "Invite created"
			},
			GuildAuditLogEvent.InviteDeleted => PreferredLanguage switch
			{
				Language.De => "Einladung gelöscht",
				Language.Fr => "Invitation supprimée",
				Language.Es => "Invitación eliminada",
				Language.Ru => "Приглашение удалено",
				Language.It => "Invito cancellato",
				_ => "Invite deleted"
			},
			GuildAuditLogEvent.ThreadCreated => PreferredLanguage switch
			{
				Language.De => "Thema erstellt",
				Language.Fr => "Fil créé",
				Language.Es => "Hilo creado",
				Language.Ru => "Тема создана",
				Language.It => "Discussione creata",
				_ => "Thread created"
			},
			GuildAuditLogEvent.VoiceJoined => PreferredLanguage switch
			{
				Language.De => "Mitglied ist einem Sprachkanal beigetreten",
				Language.Fr => "Membre rejoint le salon vocal",
				Language.Es => "Miembro se unió al canal de voz",
				Language.Ru => "Участник присоединился к голосовому каналу",
				Language.It => "Membro entrato nel canale vocale",
				_ => "Member joined voice channel",
			},
			GuildAuditLogEvent.VoiceLeft => PreferredLanguage switch
			{
				Language.De => "Mitglied hat einen Sprachkanal verlassen",
				Language.Fr => "Membre a quitté le salon vocal",
				Language.Es => "Miembro dejó el canal de voz",
				Language.Ru => "Участник покинул голосовой канал",
				Language.It => "Membro uscito dal canale vocale",
				_ => "Member left voice channel",
			},
			GuildAuditLogEvent.VoiceMoved => PreferredLanguage switch
			{
				Language.De => "Mitglied hat sich in einen anderen Sprachkanal bewegt",
				Language.Fr => "Membre a déplacé le salon vocal",
				Language.Es => "Miembro movió al canal de voz",
				Language.Ru => "Участник переместился в другой голосовой канал",
				Language.It => "Membro spostato nel canale vocale",
				_ => "Member moved voice channel",
			},
			GuildAuditLogEvent.ReactionAdded => PreferredLanguage switch
			{
				Language.De => "Reaktion hinzugefügt",
				Language.Fr => "Réaction ajoutée",
				Language.Es => "Reacción añadida",
				Language.Ru => "Реакция добавлена",
				Language.It => "Risposta aggiunta",
				_ => "Reaction added",
			},
			GuildAuditLogEvent.ReactionRemoved => PreferredLanguage switch
			{
				Language.De => "Reaktion entfernt",
				Language.Fr => "Réaction supprimée",
				Language.Es => "Reacción eliminada",
				Language.Ru => "Реакция удалена",
				Language.It => "Risposta rimossa",
				_ => "Reaction removed",
			},
			_ => "Unknown"
		};
	}
}