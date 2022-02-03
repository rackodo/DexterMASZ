using Discord;
using MASZ.AutoMods.Enums;
using MASZ.AutoMods.Extensions;
using MASZ.AutoMods.Models;
using MASZ.AutoMods.Translators;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASZ.AutoMods.Events;

public class AutoModEventAnnouncer : Event
{
	private readonly DiscordRest _discordRest;
	private readonly AutoModEventHandler _eventHandler;
	private readonly ILogger<AutoModEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public AutoModEventAnnouncer(IServiceProvider serviceProvider, AutoModEventHandler eventHandler,
		ILogger<AutoModEventAnnouncer> logger, DiscordRest discordRest)
	{
		_serviceProvider = serviceProvider;
		_eventHandler = eventHandler;
		_logger = logger;
		_discordRest = discordRest;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnAutoModEventRegistered += async (a, b, c, d, e) => await AnnounceAutoMod(a, b, c, d, e);

		_eventHandler.OnAutoModConfigCreated += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Created);

		_eventHandler.OnAutoModConfigUpdated += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Updated);

		_eventHandler.OnAutoModConfigDeleted += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Deleted);
	}

	private async Task AnnounceAutoMod(AutoModEvent modEvent, AutoModConfig punishmentsConfig, GuildConfig guildConfig,
		ITextChannel channel, IUser author)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();

		translator.SetLanguage(guildConfig);

		if (!string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for auto mod event {modEvent.GuildId}/{modEvent.Id} to {guildConfig.ModInternalNotificationWebhook}.");

			try
			{
				var embed = await modEvent.CreateInternalAutoModEmbed(guildConfig, author, channel,
					scope.ServiceProvider, punishmentsConfig.PunishmentType);
				await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} to {guildConfig.ModInternalNotificationWebhook}.");
			}
		}

		if (punishmentsConfig.SendDmNotification)
		{
			_logger.LogInformation(
				$"Sending dm notification for auto mod event {modEvent.GuildId}/{modEvent.Id} to {author.Id}.");

			try
			{
				var reason = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModType);
				var action = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModAction);
				await _discordRest.SendDmMessage(author.Id,
					translator.Get<AutoModNotificationTranslator>()
						.NotificationAutoModDm(author, channel, reason, action));
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} in dm to {author.Id}.");
			}
		}

		if (modEvent.AutoModAction is AutoModAction.ContentDeleted or AutoModAction.ContentDeletedAndCaseCreated &&
			punishmentsConfig.ChannelNotificationBehavior != AutoModChannelNotificationBehavior.NoNotification)
		{
			_logger.LogInformation(
				$"Sending channel notification to {modEvent.GuildId}/{modEvent.Id} {channel.GuildId}/{channel.Id}.");

			try
			{
				var reason = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModType);
				IMessage msg = await channel.SendMessageAsync(translator.Get<AutoModNotificationTranslator>()
					.NotificationAutoModChannel(author, reason));

				if (punishmentsConfig.ChannelNotificationBehavior ==
					AutoModChannelNotificationBehavior.SendNotificationAndDelete)
				{
					async void Action()
					{
						await Task.Delay(TimeSpan.FromSeconds(5));

						try
						{
							_logger.LogInformation($"Deleting channel auto mod event notification {channel.GuildId}/{channel.Id}/{msg.Id}.");
							await msg.DeleteAsync();
						}
						catch (UnauthorizedException)
						{
						}
						catch (Exception e)
						{
							_logger.LogError(e, $"Error while deleting message {channel.GuildId}/{channel.Id}/{msg.Id} for auto mod event {modEvent.GuildId}/{modEvent.Id}.");
						}
					}

					Task task = new(Action);

					task.Start();
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} in channel {channel.Id}.");
			}
		}
	}

	private async Task AnnounceAutoModConfig(AutoModConfig config, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"Announcing auto mod config {config.GuildId}/{config.AutoModType} ({config.Id}).");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(config.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for config {config.GuildId}/{config.AutoModType} ({config.Id}) to {guildConfig.ModInternalNotificationWebhook}.");

			try
			{
				var embed = await config.CreateAutoModConfigEmbed(actor, action, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing config  {config.GuildId}/{config.AutoModType} ({config.Id}) to {guildConfig.ModInternalNotificationWebhook}.");
			}
		}
	}
}