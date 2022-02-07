using Discord;
using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using UserNotes.Extensions;
using UserNotes.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UserNotes.Events;

public class UserNoteEventAnnouncer : Event
{
	private readonly DiscordRest _discordRest;
	private readonly UserNoteEventHandler _eventHandler;
	private readonly ILogger<UserNoteEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public UserNoteEventAnnouncer(UserNoteEventHandler eventHandler, DiscordRest discordRest,
		ILogger<UserNoteEventAnnouncer> logger, IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_discordRest = discordRest;
		_logger = logger;
		_serviceProvider = serviceProvider;
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

		if (!string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for user note {userNote.GuildId}/{userNote.UserId} ({userNote.Id}) to {guildConfig.ModInternalNotificationWebhook}.");

			try
			{
				var user = await _discordRest.FetchUserInfo(userNote.UserId, CacheBehavior.Default);
				var embed = await userNote.CreateUserNoteEmbed(action, actor, user, scope.ServiceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing user note {userNote.GuildId}/{userNote.UserId} ({userNote.Id}) to {guildConfig.ModInternalNotificationWebhook}.");
			}
		}
	}
}