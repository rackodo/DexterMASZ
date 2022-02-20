using Bot.Enums;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Bot.Translators;
using Discord;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Punishments.Models;
using Punishments.Translators;
using System.Text;

namespace Punishments.Extensions;

public static class PunishmentEmbedCreator
{
	public static async Task<EmbedBuilder> CreateModCaseEmbed(this ModCase modCase, RestAction action, IUser actor,
		IServiceProvider provider, IUser suspect = null, bool isInternal = true)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(modCase.GuildId);

		EmbedBuilder embed;

		if (isInternal)
			embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);
		else
			embed = await EmbedCreator.CreateBasicEmbed(action, provider);

		if (suspect != null)
			embed.WithThumbnailUrl(suspect.GetAvatarOrDefaultUrl());

		embed.AddField($"**{translator.Get<BotTranslator>().Description()}**", modCase.Description.Truncate(1000))
			.WithTitle($"#{modCase.CaseId} {modCase.Title}")
			.WithFooter(
				$"{translator.Get<BotTranslator>().UserId()}: {modCase.Id} | {translator.Get<PunishmentTranslator>().CaseId()}: {modCase.CaseId}")
			.AddField($"⚖️ - {translator.Get<PunishmentTranslator>().Punishment()}",
				translator.Get<PunishmentEnumTranslator>().Enum(modCase.PunishmentType), true);

		switch (action)
		{
			case RestAction.Updated:
				if (isInternal)
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseUpdateInternal(modCase, actor));
				else
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseUpdatePublic(modCase));
				break;
			case RestAction.Deleted:
				if (isInternal)
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseDeleteInternal(modCase, actor));
				else
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseDeletePublic(modCase));
				break;
			case RestAction.Created:
				if (isInternal)
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseCreateInternal(modCase, actor));
				else
					embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseCreatePublic(modCase));
				break;
		}

		if (modCase.PunishedUntil != null)
			embed.AddField($"⏰ - {translator.Get<PunishmentTranslator>().PunishedUntil()}",
				modCase.PunishedUntil.Value.ToDiscordTs(), true);

		if (modCase.Labels.Length == 0) return embed;
		StringBuilder sb = new();

		foreach (var label in modCase.Labels)
		{
			sb.Append($"`{label}` ");

			if (sb.ToString().Length > 1000)
				break;
		}

		embed.AddField($"⚖️ - {translator.Get<BotTranslator>().Labels()}", sb.ToString(),
			modCase.PunishedUntil == null);

		return embed;
	}

	public static async Task<EmbedBuilder> CreateFileEmbed(this UploadedFile file, ModCase modCase, RestAction action,
		IUser actor, IServiceProvider provider)
	{
		var translator = provider.GetService<Translation>();

		if (translator == null) return null;

		await translator.SetLanguage(modCase.GuildId);

		var embed = (await EmbedCreator.CreateBasicEmbed(action, provider, actor))
			.WithFooter($"UserId: {actor.Id} | CaseId: {modCase.CaseId}")
			.AddField($"**{translator.Get<BotTranslator>().Filename()}**", file.Name.Truncate(1000));

		switch (action)
		{
			case RestAction.Updated:
				embed.Description =
					translator.Get<PunishmentNotificationTranslator>().NotificationModCaseFileUpdate(actor);
				embed.Title =
					$"**{translator.Get<BotNotificationTranslator>().NotificationFilesUpdate().ToUpper()}** - #{modCase.CaseId} {modCase.Title}";
				break;
			case RestAction.Deleted:
				embed.Description =
					translator.Get<PunishmentNotificationTranslator>().NotificationModCaseFileDelete(actor);
				embed.Title =
					$"**{translator.Get<BotNotificationTranslator>().NotificationFilesDelete().ToUpper()}** - #{modCase.CaseId} {modCase.Title}";
				break;
			case RestAction.Created:
				embed.Description =
					translator.Get<PunishmentNotificationTranslator>().NotificationModCaseFileCreate(actor);
				embed.Title =
					$"**{translator.Get<BotNotificationTranslator>().NotificationFilesCreate().ToUpper()}** - #{modCase.CaseId} {modCase.Title}";
				break;
		}

		return embed;

	}

	public static async Task<EmbedBuilder> CreateCommentEmbed(this ModCaseComment comment, RestAction action,
		IUser actor, IServiceProvider provider)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(comment.ModCase.GuildId);

		var embed = await EmbedCreator.CreateBasicEmbed(action, provider, actor);

		switch (action)
		{
			case RestAction.Updated:
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseCommentsUpdate(actor))
					.WithTitle(
						$"**{translator.Get<PunishmentNotificationTranslator>().NotificationModCaseCommentsShortUpdate().ToUpper()}** " +
						$"- #{comment.ModCase.CaseId} {comment.ModCase.Title}");
				break;
			case RestAction.Deleted:
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseCommentsDelete(actor))
					.WithTitle(
						$"**{translator.Get<PunishmentNotificationTranslator>().NotificationModCaseCommentsShortDelete().ToUpper()}** " +
						$"- #{comment.ModCase.CaseId} {comment.ModCase.Title}");
				break;
			case RestAction.Created:
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
						.NotificationModCaseCommentsCreate(actor))
					.WithTitle(
						$"**{translator.Get<PunishmentNotificationTranslator>().NotificationModCaseCommentsShortCreate().ToUpper()}** " +
						$"- #{comment.ModCase.CaseId} {comment.ModCase.Title}");
				break;
		}

		if (actor != null)
			embed.AddField($"**{translator.Get<BotTranslator>().Message()}**", comment.Message.Truncate(1000))
				.WithFooter(
					$"{translator.Get<BotTranslator>().UserId()}: {actor.Id} | {translator.Get<PunishmentTranslator>().CaseId()}: {comment.ModCase.CaseId}");

		return embed;
	}
}