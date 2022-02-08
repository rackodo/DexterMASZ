using Bot.Abstractions;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.Events;

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
		_eventHandler.OnGuildRegistered += async (a, b) => await AnnounceTipsInNewGuild(a);
	}

	private async Task AnnounceTipsInNewGuild(GuildConfig guildConfig)
	{
		if (!string.IsNullOrEmpty(guildConfig.ModNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending Dexter.Internal tips webhook to {guildConfig.ModNotificationWebhook} for guild {guildConfig.GuildId}.");

			try
			{
				using var scope = _serviceProvider.CreateScope();

				var embed = await guildConfig.CreateTipsEmbedForNewGuilds(scope.ServiceProvider);

				await DiscordRest.ExecuteWebhook(guildConfig.ModNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing tips to {guildConfig.ModNotificationWebhook} for guild {guildConfig.GuildId}.");
			}
		}
	}
}