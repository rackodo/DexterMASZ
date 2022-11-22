using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace MOTDs.Translators;

public class MotdNotificationTranslator : Translator
{
    public string NotificationMotdInternalCreate(IUser actor) =>
        PreferredLanguage switch
        {
            Language.De => $"Neue MotD wurde von {actor.Mention} erstellt.",
            Language.Fr => $"Le nouveau MotD a été créé par {actor.Mention}.",
            Language.Es => $"El nuevo MotD ha sido creado por {actor.Mention}.",
            Language.Ru => $"Новый MotD был создан {actor.Mention}.",
            Language.It => $"Il nuovo MotD è stato creato da {actor.Mention}.",
            _ => $"New MotD has been created by {actor.Mention}."
        };

    public string NotificationMotdInternalEdited(IUser actor) =>
        PreferredLanguage switch
        {
            Language.De => $"MotD wurde von {actor.Mention} bearbeitet.",
            Language.Fr => $"MotD a été édité par {actor.Mention}.",
            Language.Es => $"MotD ha sido editado por {actor.Mention}.",
            Language.Ru => $"MotD редактировал {actor.Mention}.",
            Language.It => $"MotD è stato modificato da {actor.Mention}.",
            _ => $"MotD has been edited by {actor.Mention}."
        };

    public string NotificationMotdShow() =>
        PreferredLanguage switch
        {
            Language.De => "Anzeigen",
            Language.Fr => "Montrer",
            Language.Es => "Show",
            Language.Ru => "Показывать",
            Language.It => "Spettacolo",
            _ => "Show"
        };
}
