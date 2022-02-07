using Discord;
using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using UserMaps.Extensions;
using UserMaps.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserMaps.Events;

public class UserMapEventAnnouncer : Event
{
	private readonly UserMapEventHandler _eventHandler;
	private readonly ILogger<UserMapEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public UserMapEventAnnouncer(UserMapEventHandler eventHandler, ILogger<UserMapEventAnnouncer> logger,
		IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnUserMapCreated += async (a, b) => await AnnounceUserMap(a, b, RestAction.Created);

		_eventHandler.OnUserMapUpdated += async (a, b) => await AnnounceUserMap(a, b, RestAction.Updated);

		_eventHandler.OnUserMapDeleted += async (a, b) => await AnnounceUserMap(a, b, RestAction.Deleted);
	}

	private async Task AnnounceUserMap(UserMap userMaps, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation(
			$"Announcing user map {userMaps.GuildId}/{userMaps.UserA}-{userMaps.UserB} ({userMaps.Id}).");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(userMaps.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for user map {userMaps.GuildId}/{userMaps.UserA}-{userMaps.UserB} ({userMaps.Id}) to {guildConfig.ModInternalNotificationWebhook}.");

			try
			{
				var embed = await userMaps.CreateUserMapEmbed(action, actor, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing user map {userMaps.GuildId}/{userMaps.UserA}-{userMaps.UserB} ({userMaps.Id}) to {guildConfig.ModInternalNotificationWebhook}.");
			}
		}
	}
}