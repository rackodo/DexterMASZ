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
			Language.Es => "UserNote",
			Language.Ru => "UserNote",
			Language.It => "Nota Utente",
			_ => "UserNote"
		};
	}

	public string UserNoteId()
	{
		return PreferredLanguage switch
		{
			Language.De => "Benutzernotiz-ID",
			Language.Fr => "Identifiant de Note d'utilisateur",
			Language.Es => "ID de Nota de Usuario",
			Language.Ru => "идентификатор пользовательской заметки",
			Language.It => "ID Nota Utente",
			_ => "UserNote ID"
		};
	}
}