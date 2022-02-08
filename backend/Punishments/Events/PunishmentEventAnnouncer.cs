using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Models;
using Bot.Services;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Punishments.Enums;
using Punishments.Extensions;
using Punishments.Models;
using Punishments.Translators;

namespace Punishments.Events;

public class PunishmentEventAnnouncer : Event
{
	private readonly DiscordRest _discordRest;
	private readonly PunishmentEventHandler _eventHandler;
	private readonly ILogger<PunishmentEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public PunishmentEventAnnouncer(PunishmentEventHandler eventHandler, DiscordRest discordRest,
		ILogger<PunishmentEventAnnouncer> logger, IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_discordRest = discordRest;
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnModCaseCreated += async (a, b, c) => await AnnounceModCase(a, b, c, RestAction.Created);

		_eventHandler.OnModCaseUpdated += async (a, b, c) => await AnnounceModCase(a, b, c, RestAction.Updated);

		_eventHandler.OnModCaseDeleted += async (a, b, c) => await AnnounceModCase(a, b, c, RestAction.Deleted);

		_eventHandler.OnModCaseMarkedToBeDeleted += async (a, b, c) => await AnnounceModCase(a, b, c, RestAction.Deleted);

		_eventHandler.OnModCaseCommentCreated += async (a, b) => await AnnounceComment(a, b, RestAction.Created);

		_eventHandler.OnModCaseCommentUpdated += async (a, b) => await AnnounceComment(a, b, RestAction.Updated);

		_eventHandler.OnModCaseCommentDeleted += async (a, b) => await AnnounceComment(a, b, RestAction.Deleted);

		_eventHandler.OnFileUploaded += async (a, b, c) => await AnnounceFile(a, b, c, RestAction.Created);

		_eventHandler.OnFileDeleted += async (a, b, c) => await AnnounceFile(a, b, c, RestAction.Deleted);
	}

	private async Task AnnounceModCase(ModCase modCase, IUser actor, bool announceDm, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"Announcing mod case {modCase.Id} in guild {modCase.GuildId}.");

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();

		var caseUser = await _discordRest.FetchUserInfo(modCase.UserId, CacheBehavior.Default);

		var settings = await scope.ServiceProvider.GetRequiredService<SettingsRepository>().GetAppSettings();

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(modCase.GuildId);

		translator.SetLanguage(guildConfig);

		if (announceDm && action != RestAction.Deleted)
		{
			_logger.LogInformation(
				$"Sending dm notification to {modCase.UserId} for case {modCase.GuildId}/{modCase.CaseId}");

			try
			{
				var guild = _discordRest.FetchGuildInfo(modCase.GuildId, CacheBehavior.Default);

				var message = string.Empty;

				switch (modCase.PunishmentType)
				{
					case PunishmentType.Mute:
						if (modCase.PunishedUntil.HasValue)
							message = translator.Get<PunishmentNotificationTranslator>()
								.NotificationModCaseDmMuteTemp(modCase, guild, settings.ServiceBaseUrl);
						else
							message = translator.Get<PunishmentNotificationTranslator>()
								.NotificationModCaseDmMutePerm(guild, settings.ServiceBaseUrl);
						break;
					case PunishmentType.Kick:
						message = translator.Get<PunishmentNotificationTranslator>()
							.NotificationModCaseDmKick(guild, settings.ServiceBaseUrl);
						break;
					case PunishmentType.Ban:
						if (modCase.PunishedUntil.HasValue)
							message = translator.Get<PunishmentNotificationTranslator>()
								.NotificationModCaseDmBanTemp(modCase, guild, settings.ServiceBaseUrl);
						else
							message = translator.Get<PunishmentNotificationTranslator>()
								.NotificationModCaseDmBanPerm(guild, settings.ServiceBaseUrl);
						break;
					case PunishmentType.Warn:
						message = translator.Get<PunishmentNotificationTranslator>()
							.NotificationModCaseDmWarn(guild, settings.ServiceBaseUrl);
						break;
				}

				await _discordRest.SendDmMessage(modCase.UserId, message);
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing mod case {modCase.GuildId}/{modCase.CaseId} in DMs to {modCase.UserId}.");
			}
		}

		if (!string.IsNullOrEmpty(guildConfig.ModNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending webhook for mod case {modCase.GuildId}/{modCase.CaseId} to {guildConfig.ModNotificationWebhook}.");

			try
			{
				var embed = await modCase.CreateModCaseEmbed(action, actor, scope.ServiceProvider, caseUser);
				await DiscordRest.ExecuteWebhook(guildConfig.ModNotificationWebhook, embed.Build(),
					$"<@{modCase.UserId}>");
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing mod case {modCase.GuildId}/{modCase.CaseId} to {guildConfig.ModNotificationWebhook}.");
			}
		}
	}

	private async Task AnnounceComment(ModCaseComment comment, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation(
			$"Announcing comment {comment.Id} in case {comment.ModCase.GuildId}/{comment.ModCase.CaseId}.");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(comment.ModCase.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.ModNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending webhook for comment {comment.ModCase.GuildId}/{comment.ModCase.CaseId}/{comment.Id} to {guildConfig.ModNotificationWebhook}.");

			try
			{
				var embed = await comment.CreateCommentEmbed(action, actor, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing comment {comment.ModCase.GuildId}/{comment.ModCase.CaseId}/{comment.Id} to {guildConfig.ModNotificationWebhook}.");
			}
		}
	}

	private async Task AnnounceFile(UploadedFile file, ModCase modCase, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"Announcing file {modCase.GuildId}/{modCase.CaseId}/{file.Name}.");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(modCase.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.ModNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending webhook for file {modCase.GuildId}/{modCase.CaseId}/{file.Name} to {guildConfig.ModNotificationWebhook}.");

			try
			{
				var embed = await file.CreateFileEmbed(modCase, action, actor, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing file {modCase.GuildId}/{modCase.CaseId}/{file.Name} to {guildConfig.ModNotificationWebhook}.");
			}
		}
	}
}