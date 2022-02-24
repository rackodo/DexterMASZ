using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserMaps.Extensions;
using UserMaps.Models;

namespace UserMaps.Events;

public class UserMapEventAnnouncer : Event
{
	private readonly UserMapEventHandler _eventHandler;
	private readonly ILogger<UserMapEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly DiscordSocketClient _client;

	public UserMapEventAnnouncer(UserMapEventHandler eventHandler, ILogger<UserMapEventAnnouncer> logger,
		IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
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

		_logger.LogInformation(
			$"Sending internal webhook for user map {userMaps.GuildId}/{userMaps.UserA}-{userMaps.UserB} ({userMaps.Id}) to {guildConfig.StaffLogs}.");

		try
		{
			var embed = await userMaps.CreateUserMapEmbed(action, actor, scope.ServiceProvider);

			await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffLogs, embed);
		}
		catch (Exception e)
		{
			_logger.LogError(e,
				$"Error while announcing user map {userMaps.GuildId}/{userMaps.UserA}-{userMaps.UserB} ({userMaps.Id}) to {guildConfig.StaffLogs}.");
		}
	}
}