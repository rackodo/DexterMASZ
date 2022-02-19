using Bot.Abstractions;
using Bot.Enums;

namespace UserNotes.Translators;

public class UserNoteTranslator : Translator
{
	public string UserNote()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzernotiz",
			Language.Fr => "Note de l'utilisateur",
			Language.Es => "Usernote",
			Language.Ru => "Usernote",
			Language.It => "Nota utente",
			_ => "Usernote"
		};
	}

	public string UserNoteId()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzernotiz-ID",
			Language.Fr => "Identifiant de note d'utilisateur",
			Language.Es => "ID de nota de usuario",
			Language.Ru => "идентификатор пользовательской заметки",
			Language.It => "ID nota utente",
			_ => "Usernote ID"
		};
	}
}