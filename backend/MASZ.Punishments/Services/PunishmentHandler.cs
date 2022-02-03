using Discord.WebSocket;
using Humanizer;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Enums;
using MASZ.Bot.Events;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.Enums;
using MASZ.Punishments.Models;
using MASZ.Punishments.Translators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace MASZ.Punishments.Services;

public class PunishmentHandler : Event
{
	private readonly BotEventHandler _botEventHandler;
	private readonly DiscordSocketClient _client;
	private readonly DiscordRest _discordRest;
	private readonly ILogger<PunishmentHandler> _logger;
	private readonly IServiceProvider _serviceProvider;

	public PunishmentHandler(BotEventHandler botEventHandler, DiscordRest discordRest,
		ILogger<PunishmentHandler> logger, IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_botEventHandler = botEventHandler;
		_discordRest = discordRest;
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
	}

	public void RegisterEvents()
	{
		_botEventHandler.OnBotLaunched += StartLoopingCases;
		_client.UserJoined += HandleUserJoin;
	}

	public async Task StartLoopingCases()
	{
		try
		{
			_logger.LogWarning("Starting case loop.");

			Timer eventTimer = new(TimeSpan.FromMinutes(1).TotalMilliseconds)
			{
				AutoReset = true,
				Enabled = true
			};

			eventTimer.Elapsed += (_, _) => CheckAllCurrentPunishments();

			await Task.Run(() => eventTimer.Start());

			CheckAllCurrentPunishments();

			_logger.LogWarning("Finished case loop.");
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "Something went wrong while starting the punishment handling timer.");
		}
	}

	public async void CheckAllCurrentPunishments()
	{
		using var scope = _serviceProvider.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<PunishmentDatabase>();
		var cases = await database.SelectAllModCasesWithActivePunishments();

		foreach (var element in cases.Where(element => element.PunishedUntil != null).Where(element => element.PunishedUntil <= DateTime.UtcNow))
		{
			try
			{
				await ModifyPunishment(element, RestAction.Deleted);
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex, $"Something went wrong while undoing punishment for modcase {element.GuildId}/{element.CaseId} ({element.Id}).");
			}
			element.PunishmentActive = false;
			await database.UpdateModCase(element);
		}
	}

	public async Task ModifyPunishment(ModCase modCase, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		try
		{
			var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
				.GetGuildConfig(modCase.GuildId);

			string reason = null;

			try
			{
				var translator = scope.ServiceProvider.GetRequiredService<Translation>();
				translator.SetLanguage(guildConfig);

				reason = action switch
				{
					RestAction.Created => translator.Get<PunishmentNotificationTranslator>()
						.NotificationDiscordAuditLogPunishmentsExecute(modCase.CaseId, modCase.LastEditedByModId,
							modCase.Title.Truncate(400)),
					RestAction.Deleted => translator.Get<PunishmentNotificationTranslator>()
						.NotificationDiscordAuditLogPunishmentsUndone(modCase.CaseId, modCase.Title.Truncate(400)),
					_ => throw new NotImplementedException()
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,
					$"Failed to resolve audit log reason string for case {modCase.GuildId}/{modCase.CaseId}");
			}

			switch (modCase.PunishmentType)
			{
				case PunishmentType.Mute:
					switch (action)
					{
						case RestAction.Created:
							if (guildConfig.MutedRoles.Length != 0)
							{
								_logger.LogInformation(
									$"Mute User {modCase.UserId} in guild {modCase.GuildId} with roles " +
									string.Join(',', guildConfig.MutedRoles.Select(x => x.ToString())));

								foreach (var role in guildConfig.MutedRoles)
									await _discordRest.GrantGuildUserRole(modCase.GuildId, modCase.UserId, role,
										reason);
							}
							else
							{
								_logger.LogInformation(
									$"Cannot Mute User {modCase.UserId} in guild {modCase.GuildId} - mute role undefined.");
							}

							break;
						case RestAction.Deleted:
							if (guildConfig.MutedRoles.Length != 0)
							{
								_logger.LogInformation(
									$"Un mute User {modCase.UserId} in guild {modCase.GuildId} with roles " +
									string.Join(',', guildConfig.MutedRoles.Select(x => x.ToString())));

								foreach (var role in guildConfig.MutedRoles)
									await _discordRest.RemoveGuildUserRole(modCase.GuildId, modCase.UserId, role,
										reason);
							}
							else
							{
								_logger.LogInformation(
									$"Cannot Un mute User {modCase.UserId} in guild {modCase.GuildId} - mute role undefined.");
							}

							break;
					}

					break;
				case PunishmentType.Ban:
					switch (action)
					{
						case RestAction.Created:
							_logger.LogInformation($"Ban User {modCase.UserId} in guild {modCase.GuildId}.");
							await _discordRest.BanUser(modCase.GuildId, modCase.UserId, reason);
							await _discordRest.GetGuildUserBan(modCase.GuildId, modCase.UserId, CacheBehavior.IgnoreCache);
							break;
						case RestAction.Deleted:
							_logger.LogInformation($"Un ban User {modCase.UserId} in guild {modCase.GuildId}.");
							await _discordRest.UnBanUser(modCase.GuildId, modCase.UserId, reason);
							_discordRest.RemoveFromCache(CacheKey.GuildBan(modCase.GuildId, modCase.UserId));
							break;
					}
					break;
				case PunishmentType.Kick:
					switch (action)
					{
						case RestAction.Created:
							_logger.LogInformation($"Kick User {modCase.UserId} in guild {modCase.GuildId}.");
							await _discordRest.KickGuildUser(modCase.GuildId, modCase.UserId, reason);
							break;
					}

					break;
			}
		}
		catch (ResourceNotFoundException)
		{
			_logger.LogError($"Cannot execute punishment in guild {modCase.GuildId} - guild config not found.");
		}
	}

	public async Task HandleUserJoin(SocketGuildUser user)
	{
		using var scope = _serviceProvider.CreateScope();
		var database = scope.ServiceProvider.GetRequiredService<PunishmentDatabase>();

		try
		{
			var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
				.GetGuildConfig(user.Guild.Id);

			if (guildConfig.MutedRoles.Length == 0)
				return;

			var modCases = await database.SelectAllModCasesWithActiveMuteForGuildAndUser(user.Guild.Id, user.Id);

			if (modCases.Count == 0)
				return;

			_logger.LogInformation($"Muted user {user.Id} rejoined guild {user.Guild.Id}");

			await ModifyPunishment(modCases[0], RestAction.Created);
		}
		catch (ResourceNotFoundException)
		{
			_logger.LogInformation($"Cannot execute punishment in guild {user.Guild.Id} - guild config not found.");
		}
	}
}