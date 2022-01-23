using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Extensions;

namespace MASZ.Invites.Translators;

public class InviteNotificationTranslator : Translator
{
	public string NotificationAutoWhoisJoinWith(IUser user, DateTime registered, string invite)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"{user.Mention} (registriert {registered.ToDiscordTs()}) ist mit dem Invite `{invite}` beigetreten.",
			Language.At =>
				$"{user.Mention} (registriat {registered.ToDiscordTs()}) is mit da Eiladung `{invite}` beigetretn.",
			Language.Fr =>
				$"{user.Mention} (enregistré {registered.ToDiscordTs()}) rejoint avec l'invitation `{invite}`.",
			Language.Es =>
				$"{user.Mention} (registrado {registered.ToDiscordTs()}) se unió con la invitación `{invite}`.",
			Language.Ru =>
				$"{user.Mention} (зарегистрированный {registered.ToDiscordTs()}) присоединился с приглашением `{invite}`.",
			Language.It =>
				$"{user.Mention} (registrato {registered.ToDiscordTs()}) si è unito con l'invito `{invite}`.",
			_ => $"{user.Mention} (registered {registered.ToDiscordTs()}) joined with invite `{invite}`."
		};
	}

	public string NotificationAutoWhoisJoinWithAndFrom(IUser user, ulong by, DateTime createdAt, DateTime registered,
		string invite)
	{
		return PreferredLanguage switch
		{
			Language.De =>
				$"{user.Mention} (registriert {registered.ToDiscordTs()}) ist mit dem Invite `{invite}` von <@{by}> (am {createdAt.ToDiscordTs()}) beigetreten.",
			Language.At =>
				$"{user.Mention} (registriert {registered.ToDiscordTs()}) is mit da Eiladung `{invite}` vo <@{by}> (am {createdAt.ToDiscordTs()}) beigetretn.",
			Language.Fr =>
				$"{user.Mention} (enregistré {registered.ToDiscordTs()}) rejoint avec invite `{invite}` (créé {createdAt.ToDiscordTs()}) par <@{by}>.",
			Language.Es =>
				$"{user.Mention} (registrado {registered.ToDiscordTs()}) se unió con la invitación `{invite}` (creado {createdAt.ToDiscordTs()}) por <@{by}>.",
			Language.Ru =>
				$"{user.Mention} (зарегистрированный {registered.ToDiscordTs()}) присоединился с помощью приглашения `{invite}` (created {createdAt.ToDiscordTs()}) пользователем <@{by}>.",
			Language.It =>
				$"{user.Mention} (registrato {registered.ToDiscordTs()}) si è unito all'invito `{invite}` (creato {createdAt.ToDiscordTs()}) da <@{by}>.",
			_ =>
				$"{user.Mention} (registered {registered.ToDiscordTs()}) joined with invite `{invite}` (created {createdAt.ToDiscordTs()}) by <@{by}>."
		};
	}
}