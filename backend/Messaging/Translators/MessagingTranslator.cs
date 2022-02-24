using Bot.Abstractions;
using Bot.Enums;
using Discord;

namespace Messaging.Translators;

public class MessagingTranslator : Translator
{
	public string SaySent(IUser user, IMentionable channel)
	{
		return PreferredLanguage switch
		{
			Language.De => $"{user.Mention} verwendete den Say-Befehl in {channel.Mention}.",
			Language.Fr => $"{user.Mention} a utilisé la commande « dire » dans{channel.Mention}.",
			Language.Es => $"{user.Mention} usó el comando «decir» en {channel.Mention}.",
			Language.Ru => $"{user.Mention} использовал команду «сказать» в {channel.Mention}.",
			Language.It => $"{user.Mention} ha usato il comando \"dire\" in {channel.Mention}.",
			_ => $"{user.Mention} used the say command in {channel.Mention}.",
		};
	}

	public string FailedToSend()
	{
		return PreferredLanguage switch
		{
			Language.De => "Senden der Nachricht fehlgeschlagen",
			Language.Fr => "Échec de l'envoi du message",
			Language.Es => "No se pudo enviar el mensaje",
			Language.Ru => "Не удалось отправить сообщение",
			Language.It => "Impossibile inviare il messaggio",
			_ => "Failed to send message"
		};
	}

	public string MessageSent()
	{
		return PreferredLanguage switch
		{
			Language.De => "Nachricht gesendet.",
			Language.Fr => "Message envoyé.",
			Language.Es => "Mensaje enviado.",
			Language.Ru => "Сообщение отправлено.",
			Language.It => "Messaggio inviato.",
			_ => "Message sent."
		};
	}
}
