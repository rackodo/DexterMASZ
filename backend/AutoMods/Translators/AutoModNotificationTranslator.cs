using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace AutoMods.Translators;

public class AutoModNotificationTranslator : Translator
{
    public string NotificationAutoModInternal(IUser user) =>
        PreferredLanguage switch
        {
            Language.De => $"{user.Mention} hat die automod ausgelöst.",
            Language.Fr => $"{user.Mention} a déclenché l'automodération.",
            Language.Es => $"{user.Mention} activó la automoderación.",
            Language.Ru => $"{user.Mention} запустил автомодерацию.",
            Language.It => $"{user.Mention} ha attivato la moderazione automatica.",
            _ => $"{user.Mention} triggered the automod."
        };

    public string NotificationAutoModCase(IUser user) =>
        PreferredLanguage switch
        {
            Language.De => $"{user.Username}#{user.Discriminator} hat die automod ausgelöst.",
            Language.Fr => $"{user.Username}#{user.Discriminator} a déclenché la modération automatique.",
            Language.Es => $"{user.Username}#{user.Discriminator} desencadenó la automoderación.",
            Language.Ru => $"{user.Username}#{user.Discriminator} запускает автомодерацию.",
            Language.It => $"{user.Username}#{user.Discriminator} ha attivato la moderazione automatica.",
            _ => $"{user.Username}#{user.Discriminator} triggered the automod."
        };

    public string NotificationAutoModDm(IUser user, IMentionable channel, string reason, string action) =>
        PreferredLanguage switch
        {
            Language.De =>
                $"Hallo {user.Mention},\n\nDu hast die automoderation in {channel.Mention} ausgelöst.\nGrund: **{reason}**\nAktion: {action}",
            Language.Fr =>
                $"Salut {user.Mention},\n\nVous avez déclenché l'automodération dans {channel.Mention}.\nRaison : **{reason}**\nAction : {action}",
            Language.Es =>
                $"Hola, {user.Mention}:\n\nActivó la automoderación en {channel.Mention}.\nMotivo: **{reason}**\nAcción: {action}",
            Language.Ru =>
                $"Привет, {user.Mention}!\n\nВы активировали автомодерацию в {channel.Mention}.\nПричина: **{reason}**\nДействие: {action}",
            Language.It =>
                $"Ciao {user.Mention},\n\nHai attivato la moderazione automatica in {channel.Mention}.\nMotivo: **{reason}**\nAzione: {action}",
            _ =>
                $"Hi {user.Mention},\n\nYou triggered the automod in {channel.Mention}.\nReason: **{reason}**\nAction: {action}"
        };

    public string NotificationAutoModChannel(IUser user, string reason) =>
        PreferredLanguage switch
        {
            Language.De =>
                $"{user.Mention} du hast die automoderation ausgelöst. Grund: **{reason}**. Deine Nachricht wurde gelöscht.",
            Language.Fr =>
                $"{user.Mention} vous avez déclenché l'automodération. Raison : **{reason}**. Votre message a été supprimé.",
            Language.Es =>
                $"{user.Mention} has activado la automoderación. Razón: **{reason}**. Su mensaje ha sido eliminado.",
            Language.Ru =>
                $"{user.Mention} вы запустили автомодерацию. Причина: **{reason}**. Ваше сообщение было удалено.",
            Language.It =>
                $"{user.Mention} hai attivato la moderazione automatica. reason: **{reason}**. Il tuo messaggio è stato cancellato.",
            _ => $"{user.Mention} you triggered the automod! Reason: **{reason}**. Your message has been deleted."
        };

    public string NotificationAutoModConfigInternalCreate(string eventType, IUser actor) =>
        PreferredLanguage switch
        {
            Language.De => $"Auto mod konfiguration für **{eventType}** von {actor.Mention} erstellt.",
            Language.Fr => $"Mode automatique config créé pour **{eventType}** par {actor.Mention}.",
            Language.Es => $"Modo automático config creado para **{eventType}** por {actor.Mention}.",
            Language.Ru => $"Конфигурация авто мода, созданный для **{eventType}** пользователем {actor.Mention}.",
            Language.It => $"Configurazione mod automatica creato per **{eventType}** da {actor.Mention}.",
            _ => $"Auto mod config created for **{eventType}** by {actor.Mention}."
        };

    public string NotificationAutoModConfigInternalUpdate(string eventType, IUser actor) =>
        PreferredLanguage switch
        {
            Language.De => $"Auto mod konfiguration für **{eventType}** von {actor.Mention} bearbeitet.",
            Language.Fr => $"Mode automatique config mis à jour pour **{eventType}** par {actor.Mention}.",
            Language.Es => $"Modo automático config actualizado para **{eventType}** por {actor.Mention}.",
            Language.Ru => $"Конфигурация авто мода обновлен для **{eventType}** пользователем {actor.Mention}.",
            Language.It => $"Configurazione mod automatica aggiornato per **{eventType}** da {actor.Mention}.",
            _ => $"Auto mod config updated for **{eventType}** by {actor.Mention}."
        };

    public string NotificationAutoModConfigInternalDelete(string eventType, IUser actor) =>
        PreferredLanguage switch
        {
            Language.De => $"Auto mod konfiguration für **{eventType}** von {actor.Mention} gelöscht.",
            Language.Fr => $"Mode automatique config supprimé pour **{eventType}** par {actor.Mention}.",
            Language.Es => $"Modo automático config eliminado para **{eventType}** por {actor.Mention}.",
            Language.Ru => $"Конфигурация авто мода удален для **{eventType}** пользователем {actor.Mention}.",
            Language.It => $"Configurazione mod automatica eliminato per **{eventType}** da {actor.Mention}.",
            _ => $"Auto mod config deleted for **{eventType}** by {actor.Mention}."
        };

    public string NotificationAutoModConfigLimit() =>
        PreferredLanguage switch
        {
            Language.De => "Limit",
            Language.Fr => "Limite",
            Language.Es => "Límite",
            Language.Ru => "Предел",
            Language.It => "Limite",
            _ => "Limit"
        };

    public string NotificationAutoModConfigTimeLimit() =>
        PreferredLanguage switch
        {
            Language.De => "Zeitlimit",
            Language.Fr => "Limite de temps",
            Language.Es => "Límite de tiempo",
            Language.Ru => "Лимит времени",
            Language.It => "Limite di tempo",
            _ => "Time limit"
        };

    public string NotificationAutoModConfigIgnoredRoles() =>
        PreferredLanguage switch
        {
            Language.De => "Ignorierte Rollen",
            Language.Fr => "Rôles ignorés",
            Language.Es => "Roles ignorados",
            Language.Ru => "Игнорируемые роли",
            Language.It => "Ruoli ignorati",
            _ => "Ignored roles"
        };

    public string NotificationAutoModConfigIgnoredChannels() =>
        PreferredLanguage switch
        {
            Language.De => "Ignorierte Kanäle",
            Language.Fr => "Canaux ignorés",
            Language.Es => "Canales ignorados",
            Language.Ru => "Игнорируемые каналы",
            Language.It => "Canali ignorati",
            _ => "Ignored channels"
        };

    public string NotificationAutoModConfigDuration() =>
        PreferredLanguage switch
        {
            Language.De => "Dauer",
            Language.Fr => "Durée",
            Language.Es => "Duración",
            Language.Ru => "Продолжительность",
            Language.It => "Durata",
            _ => "Duration"
        };

    public string NotificationAutoModConfigDeleteMessage() =>
        PreferredLanguage switch
        {
            Language.De => "Nachricht löschen",
            Language.Fr => "Supprimer le message",
            Language.Es => "Borrar mensaje",
            Language.Ru => "Удаленное сообщение",
            Language.It => "Cancella il messaggio",
            _ => "Delete message"
        };

    public string NotificationAutoModConfigSendPublic() =>
        PreferredLanguage switch
        {
            Language.De => "Sende öffentliche Nachricht",
            Language.Fr => "Envoyer une notification publique",
            Language.Es => "Enviar notificación pública",
            Language.Ru => "Отправить публичное уведомление",
            Language.It => "Invia notifica pubblica",
            _ => "Send public notification"
        };

    public string NotificationAutoModConfigSendDm() =>
        PreferredLanguage switch
        {
            Language.De => "Sende DM Nachricht",
            Language.Fr => "Envoyer une notification DM",
            Language.Es => "Enviar notificación DM",
            Language.Ru => "Отправить уведомление в прямом эфире",
            Language.It => "Invia notifica DM",
            _ => "Send DM notification"
        };
}
