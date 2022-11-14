using Bot.Abstractions;
using Bot.Enums;

namespace UserNotes.Translators;

public class UserNoteTranslator : Translator
{
    public string UserNote() =>
        PreferredLanguage switch
        {
            Language.De => "Benutzernotiz",
            Language.Fr => "Note de l'utilisateur",
            Language.Es => "User Note",
            Language.Ru => "User Note",
            Language.It => "Nota Utente",
            _ => "User Note"
        };

    public string UserNoteId() =>
        PreferredLanguage switch
        {
            Language.De => "Benutzernotiz-ID",
            Language.Fr => "Identifiant de Note d'utilisateur",
            Language.Es => "ID de Nota de Usuario",
            Language.Ru => "идентификатор пользовательской заметки",
            Language.It => "ID Nota Utente",
            _ => "User Note ID"
        };
}