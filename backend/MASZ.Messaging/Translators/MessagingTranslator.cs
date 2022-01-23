using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;

namespace MASZ.Messaging.Translators;

public class MessagingTranslator : Translator
{
	public string SaySent(IUser user, IUserMessage message, IMentionable channel)
	{
		return PreferredLanguage switch
		{
			Language.De => $"{user.Mention} verwendete den Say-Befehl in {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.At => $"{user.Mention} verwendete den Say-Befehl in {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Fr => $"{user.Mention} a utilisé la commande « dire » dans{channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Es => $"{user.Mention} usó el comando «decir» en {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.Ru => $"{user.Mention} использовал команду «сказать» в {channel.Mention}.\n{message.GetJumpUrl()}",
			Language.It => $"{user.Mention} ha usato il comando \"dire\" in {channel.Mention}.\n{message.GetJumpUrl()}",
			_ => $"{user.Mention} used the say command in {channel.Mention}.\n{message.GetJumpUrl()}",
		};
	}

	public string FailedToSend()
	{
		return PreferredLanguage switch
		{
			Language.De => "Senden der Nachricht fehlgeschlagen",
			Language.At => "Sendn vo da Nachricht fehlgschlogn.",
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
			Language.At => "Nochricht gsendet.",
			Language.Fr => "Message envoyé.",
			Language.Es => "Mensaje enviado.",
			Language.Ru => "Сообщение отправлено.",
			Language.It => "Messaggio inviato.",
			_ => "Message sent."
		};
	}
}
