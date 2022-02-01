using Discord;
using MASZ.AutoMods.Enums;
using MASZ.AutoMods.Models;
using MASZ.AutoMods.Translators;
using MASZ.Bot.Enums;
using MASZ.Bot.Extensions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using MASZ.Bot.Translators;
using MASZ.Punishments.Enums;
using MASZ.Punishments.Translators;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.AutoMods.Extensions;

public static class AutoModEmbedCreator
{
	public static async Task<EmbedBuilder> CreateInternalAutoModEmbed(this AutoModEvent autoModEvent,
		GuildConfig guildConfig,
		IUser user, ITextChannel channel, IServiceProvider provider, PunishmentType? punishmentType = null)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(autoModEvent.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(RestAction.Created, provider);

		embed.WithTitle(translator.Get<AutoModTranslator>().AutoModeration())
			.WithDescription(translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationInternal(user))
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
			).AddField(
				translator.Get<BotTranslator>().MessageContent(),
				autoModEvent.MessageContent
			).AddField(
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

		if (actor != null)
			embed.WithThumbnailUrl(actor.GetAvatarOrDefaultUrl());

		embed.WithTitle(translator.Get<AutoModTranslator>().AutoModeration() + ": " +
		                translator.Get<AutoModEnumTranslator>().Enum(autoModConfig.AutoModType));

		var autoModTypeName = translator.Get<AutoModEnumTranslator>().Enum(autoModConfig.AutoModType);

		switch (action)
		{
			case RestAction.Created:
				embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModerationConfigInternalCreate(autoModTypeName, actor));
				break;
			case RestAction.Updated:
				embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModerationConfigInternalUpdate(autoModTypeName, actor));
				break;
			case RestAction.Deleted:
				return embed.WithDescription(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModerationConfigInternalDelete(autoModTypeName, actor));
		}

		if (autoModConfig.Limit != null)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigLimit(),
				$"`{autoModConfig.Limit}`", true);

		if (autoModConfig.TimeLimitMinutes != null)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigTimeLimit(),
				$"`{autoModConfig.TimeLimitMinutes}`", true);

		if (autoModConfig.Limit != null || autoModConfig.TimeLimitMinutes != null)
			embed.AddField("​", "​"); // ZERO WIDTH SPACE

		if (autoModConfig.IgnoreRoles.Length > 0)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigIgnoredRoles(),
				string.Join(" ", autoModConfig.IgnoreRoles.Select(x => $"<@&{x}>")), true);

		if (autoModConfig.IgnoreChannels.Length > 0)
			embed.AddField(translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigIgnoredChannels(),
				string.Join(" ", autoModConfig.IgnoreChannels.Select(x => $"<#{x}>")), true);

		if (autoModConfig.IgnoreRoles.Length > 0 || autoModConfig.IgnoreChannels.Length > 0)
			embed.AddField("​", "​"); // ZERO WIDTH SPACE

		if (autoModConfig.PunishmentType != null &&
		    autoModConfig.AutoModAction is AutoModAction.CaseCreated or AutoModAction.ContentDeletedAndCaseCreated)
		{
			embed.AddField($"⚖ {translator.Get<PunishmentTranslator>().Punishment()}",
				translator.Get<PunishmentEnumTranslator>().Enum(autoModConfig.PunishmentType.Value), true);

			if (autoModConfig.PunishmentDurationMinutes > 0)
				embed.AddField(
					$"⏰ {translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigDuration()}",
					$"`{autoModConfig.PunishmentDurationMinutes}`", true);

			embed.AddField(
				translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigSendPublic(),
				autoModConfig.SendPublicNotification.GetCheckEmoji(),
				true);

			embed.AddField(
				translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigSendDm(),
				autoModConfig.SendDmNotification.GetCheckEmoji(),
				true);
		}

		embed.AddField(
			translator.Get<AutoModNotificationTranslator>().NotificationAutoModerationConfigDeleteMessage(),
			(autoModConfig.AutoModAction is AutoModAction.ContentDeleted or AutoModAction.ContentDeletedAndCaseCreated).GetCheckEmoji(),
			true);

		return embed;
	}
}