using Bot.Enums;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Bot.Translators;
using Discord;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Models;
using Punishments.Translators;
using System.Text;

namespace Punishments.Extensions;

public static class PunishmentEmbedCreator
{
	public static async Task<EmbedBuilder> CreateNewModCaseEmbed(this ModCase modCase, IUser actor, GuildConfig config,
		AnnouncementResult result, IServiceProvider provider, IUser suspect)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(modCase.GuildId);

		translator.SetLanguage(config);

		var embed = await modCase.CreateModCaseEmbed(RestAction.Created, actor, provider, suspect);

		if (result != AnnouncementResult.None)
			embed.AddField($"📣 - {translator.Get<PunishmentTranslator>().DMReceipt()}",
				translator.Get<PunishmentEnumTranslator>().Enum(result), true);

		return embed;
	}

	public static async Task<EmbedBuilder> CreateModCaseEmbed(this ModCase modCase, RestAction action, IUser actor,
		IServiceProvider provider, IUser suspect = null)
	{
		var translator = provider.GetRequiredService<Translation>();

		await translator.SetLanguage(modCase.GuildId);

		var embed = await EmbedCreator.CreateActionEmbed(action, provider, actor);

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
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
					.NotificationModCaseUpdate(modCase, actor));
				break;
			case RestAction.Deleted:
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
					.NotificationModCaseDelete(modCase, actor));
				break;
			case RestAction.Created:
				embed.WithDescription(translator.Get<PunishmentNotificationTranslator>()
					.NotificationModCase(modCase, actor));
				break;
		}

		var modCaseRepo = provider.GetRequiredService<ModCaseRepository>();

		var cases = await modCaseRepo.GetCasesForGuildAndUser(modCase.GuildId, modCase.UserId);

		var caseCount = cases.Count;

		embed.AddField($"📝 - {translator.Get<PunishmentTranslator>().CaseCount()}", caseCount, true);

		if (modCase.Severity != SeverityType.None)
			embed.AddField($"⚠️ - {translator.Get<PunishmentTranslator>().Severity()}",
					translator.Get<PunishmentEnumTranslator>().Enum(modCase.Severity), true);

		if (modCase.PunishedUntil != null)
			embed.AddField($"⏰ - {translator.Get<PunishmentTranslator>().PunishedUntil()}",
				modCase.PunishedUntil.Value.ToDiscordTs(), true);

		if (modCase.Labels.Length == 0)
			return embed;
		
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

	public static async Task<EmbedBuilder> CreateReportEmbed(this IMessage message, IUser user, IServiceProvider provider)
	{
		if (message is not ITextChannel channel)
			return null;

		var translation = provider.GetRequiredService<Translation>();

		await translation.SetLanguage(channel.GuildId);

		var embed = await EmbedCreator.CreateActionEmbed(RestAction.Created, provider);

		embed.WithTitle(translation.Get<PunishmentTranslator>().ReportCreated())
		.WithAuthor(user)
		.WithThumbnailUrl(message.Author.GetAvatarOrDefaultUrl())
		.WithDescription(translation.Get<PunishmentTranslator>().ReportContent(user, message, channel))
		.AddField(
			translation.Get<BotTranslator>().Channel(),
			channel.Mention,
			true
		).AddField(
			translation.Get<BotTranslator>().Message(),
			message,
			true
		).AddField(
			translation.Get<BotTranslator>().MessageUrl(),
			message.GetJumpUrl())
		.WithFooter($"{translation.Get<BotTranslator>().GuildId()}: {channel.GuildId}");

		if (message.Attachments.Count > 0)
		{
			StringBuilder sb = new();

			foreach (var attachment in message.Attachments.Take(5))
				sb.Append($"- <{attachment.Url}>\n");

			if (message.Attachments.Count > 5)
				sb.AppendLine(translation.Get<BotTranslator>().AndXMore(message.Attachments.Count - 5));

			embed.AddField(translation.Get<BotTranslator>().Attachments(), sb.ToString());
		}

		return embed;
	}

	public static async Task<EmbedBuilder> CreateFileEmbed(this UploadedFile file, ModCase modCase, RestAction action,
		IUser actor, IServiceProvider provider)
	{
		var translator = provider.GetService<Translation>();

		await translator.SetLanguage(modCase.GuildId);

		var embed = (await EmbedCreator.CreateActionEmbed(action, provider, actor))
			.WithThumbnailUrl(actor.GetAvatarOrDefaultUrl())
			.WithFooter($"{translator.Get<BotTranslator>().UserId()}: {actor.Id} | {translator.Get<PunishmentTranslator>().CaseId()}: {modCase.CaseId}")
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

		var embed = await EmbedCreator.CreateActionEmbed(action, provider, actor);

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