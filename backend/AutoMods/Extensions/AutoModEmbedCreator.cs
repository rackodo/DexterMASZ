using AutoMods.Enums;
using AutoMods.Models;
using AutoMods.Translators;
using Bot.Enums;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Bot.Translators;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Punishments.Enums;
using Punishments.Translators;

namespace AutoMods.Extensions;

public static class AutoModEmbedCreator
{
	public static async Task<EmbedBuilder> CreateInternalAutoModEmbed(this AutoModEvent autoModEvent,
		GuildConfig guildConfig,
		IUser user, ITextChannel channel, IServiceProvider provider, PunishmentType? punishmentType = null)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(autoModEvent.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(RestAction.Created, provider);

		embed.WithTitle(translator.Get<AutoModTranslator>().AutoMod())
			.WithAuthor(user)
			.WithDescription(translator.Get<AutoModNotificationTranslator>().NotificationAutoModInternal(user))
			.AddField(
				translator.Get<BotTranslator>().Channel(),
				channel.Mention,
				true
			).AddField(
				translator.Get<BotTranslator>().Message(),
				$"[{autoModEvent.MessageId}](https://discord.com/channels/{autoModEvent.GuildId}/{channel.Id}/{autoModEvent.MessageId})",
				true
			).AddField(
				translator.Get<BotTranslator>().Type(),
				translator.Get<AutoModEnumTranslator>().Enum(autoModEvent.AutoModType)
			);

		if (autoModEvent.MessageContent.Length > 0)
			embed.AddField(
				translator.Get<BotTranslator>().MessageContent(),
				autoModEvent.MessageContent
			);

		embed.AddField(
				translator.Get<BotTranslator>().Action(),
				translator.Get<AutoModEnumTranslator>().Enum(autoModEvent.AutoModAction)
			).WithFooter($"{translator.Get<BotTranslator>().GuildId()}: {guildConfig.GuildId}");

		if (autoModEvent.AutoModAction is AutoModAction.CaseCreated or AutoModAction.ContentDeletedAndCaseCreated && punishmentType != null)
			embed.AddField(translator.Get<PunishmentTranslator>().Punishment(),
				translator.Get<PunishmentEnumTranslator>().Enum(punishmentType.Value));

		return embed;
	}

	public static async Task<EmbedBuilder> CreateAutoModConfigEmbed(this AutoModConfig autoModConfig,
		IUser actor, RestAction action, IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(autoModConfig.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		embed.WithTitle(translator.Get<AutoModTranslator>().AutoMod() + ": " +
						translator.Get<AutoModEnumTranslator>().Enum(autoModConfig.AutoModType));

		var autoModTypeName =
			$"**{translator.Get<AutoModEnumTranslator>().Enum(autoModConfig.AutoModType).ToLower()}**";

		switch (action)
		{
			case RestAction.Created:
				embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModConfigInternalCreate(autoModTypeName, actor));
				break;
			case RestAction.Updated:
				embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModConfigInternalUpdate(autoModTypeName, actor));
				break;
			case RestAction.Deleted:
				return embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModConfigInternalDelete(autoModTypeName, actor));
		}

		// config info

		if (autoModConfig.Limit != null)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigLimit(),
				$"{autoModConfig.Limit}", true);

		if (autoModConfig.TimeLimitMinutes != null)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigTimeLimit(),
				$"{autoModConfig.TimeLimitMinutes}", true);

		// uid info

		if (autoModConfig.IgnoreRoles.Length > 0)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigIgnoredRoles(),
				string.Join(" ", autoModConfig.IgnoreRoles.Select(x => $"<@&{x}>")), true);

		if (autoModConfig.IgnoreChannels.Length > 0)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigIgnoredChannels(),
				string.Join(" ", autoModConfig.IgnoreChannels.Select(x => $"<#{x}>")), true);

		// punishment info

		if (autoModConfig.PunishmentType != null &&
			autoModConfig.AutoModAction is AutoModAction.CaseCreated or AutoModAction.ContentDeletedAndCaseCreated)
		{
			embed.AddField($"⚖ {translator.Get<PunishmentTranslator>().Punishment()}",
				translator.Get<PunishmentEnumTranslator>().Enum(autoModConfig.PunishmentType.Value), true);

			if (autoModConfig.PunishmentDurationMinutes > 0)
				embed.AddField(
					$"⏰ {translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigDuration()}",
					$"{autoModConfig.PunishmentDurationMinutes}", true);

			embed.AddField(
				translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigSendDm(),
				autoModConfig.SendDmNotification.GetCheckEmoji(),
				true);
		}

		embed.AddField(
			translator.Get<AutoModNotificationTranslator>().NotificationAutoModConfigDeleteMessage(),
			(autoModConfig.AutoModAction is AutoModAction.ContentDeleted or AutoModAction.ContentDeletedAndCaseCreated).GetCheckEmoji(),
			true);

		return embed;
	}
}