using Bot.Abstractions;
using Bot.Enums;

namespace Invites.Translators;

public class InviteNotificationTranslator : Translator
{
    public string Registered() =>
        PreferredLanguage switch
        {
            Language.De => "Registriert",
            Language.Fr => "Enregistré",
            Language.Es => "Registrado",
            Language.Ru => "зарегистрированный",
            Language.It => "Registrato",
            _ => "Registered"
        };

    public string Invite() =>
        PreferredLanguage switch
        {
            Language.De => "Invite",
            Language.Fr => "L'invitation",
            Language.Es => "La Invitación",
            Language.Ru => "Приглашать",
            Language.It => "L'invito",
            _ => "Invite"
        };

    public string Created() =>
        PreferredLanguage switch
        {
            Language.De => "Erstellt",
            Language.Fr => "Créé",
            Language.Es => "Creada",
            Language.Ru => "Созданный",
            Language.It => "Creata",
            _ => "Created"
        };

    public string By() =>
        PreferredLanguage switch
        {
            Language.De => "Durch",
            Language.Fr => "Par",
            Language.Es => "Por",
            Language.Ru => "К",
            Language.It => "Di",
            _ => "By"
        };
}
