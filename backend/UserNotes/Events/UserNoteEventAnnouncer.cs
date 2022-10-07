using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserNotes.Extensions;
using UserNotes.Models;

namespace UserNotes.Events;

public class UserNoteEventAnnouncer : Event
{
	private readonly DiscordRest _discordRest;
	private readonly UserNoteEventHandler _eventHandler;
	private readonly ILogger<UserNoteEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly DiscordSocketClient _client;

	public UserNoteEventAnnouncer(DiscordRest discordRest, UserNoteEventHandler eventHandler,
		ILogger<UserNoteEventAnnouncer> logger, IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_discordRest = discordRest;
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnUserNoteCreated += async (a, b) => await AnnounceUserNote(a, b, RestAction.Created);

		_eventHandler.OnUserNoteUpdated += async (a, b) => await AnnounceUserNote(a, b, RestAction.Updated);

		_eventHandler.OnUserNoteDeleted += async (a, b) => await AnnounceUserNote(a, b, RestAction.Deleted);
	}

	private async Task AnnounceUserNote(UserNote userNote, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"Announcing user note {userNote.GuildId}/{userNote.UserId} ({userNote.Id}).");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(userNote.GuildId);

		_logger.LogInformation(
			$"Sending internal webhook for user note {userNote.GuildId}/{userNote.UserId} ({userNote.Id}) to {guildConfig.StaffLogs}.");

		try
		{
			var user = await _discordRest.FetchUserInfo(userNote.UserId, false);
			var embed = await userNote.CreateUserNoteEmbed(action, actor, user, scope.ServiceProvider);

			await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffLogs, embed);
		}
		catch (Exception e)
		{
			_logger.LogError(e,
				$"Error while announcing user note {userNote.GuildId}/{userNote.UserId} ({userNote.Id}) to {guildConfig.StaffLogs}.");
		}
	}
}