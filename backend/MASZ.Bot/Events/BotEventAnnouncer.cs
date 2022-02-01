using MASZ.Bot.Abstractions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASZ.Bot.Events;

public class BotEventAnnouncer : Event
{
	private readonly BotEventHandler _eventHandler;
	private readonly ILogger<BotEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public BotEventAnnouncer(BotEventHandler eventHandler, ILogger<BotEventAnnouncer> logger,
		IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnGuildRegistered += async (a, b) => await AnnounceTipsInNewGuild(a, b);
	}

	private async Task AnnounceTipsInNewGuild(GuildConfig guildConfig, bool importExistingBans)
	{
		if (!string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending MASZ.Internal tips webhook to {guildConfig.ModInternalNotificationWebhook} for guild {guildConfig.GuildId}.");

			try
			{
				using var scope = _serviceProvider.CreateScope();

				var embed = await guildConfig.CreateTipsEmbedForNewGuilds(scope.ServiceProvider);

				await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing tips to {guildConfig.ModInternalNotificationWebhook} for guild {guildConfig.GuildId}.");
			}
		}
	}
}