using Discord;
using Discord.Net;
using Discord.WebSocket;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Invites.Data;
using MASZ.Invites.Events;
using MASZ.Invites.Models;
using MASZ.Invites.Translators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MASZ.Invites.Services;

public class InviteTracker : Event
{
	private readonly DiscordSocketClient _client;
	private readonly InviteEventHandler _eventHandler;
	private readonly Dictionary<ulong, List<TrackedInvite>> _guildInvites;
	private readonly ILogger<InviteTracker> _logger;
	private readonly IServiceProvider _serviceProvider;

	public InviteTracker(ILogger<InviteTracker> logger, IServiceProvider serviceProvider, DiscordSocketClient client,
		InviteEventHandler eventHandler)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
		_eventHandler = eventHandler;

		_guildInvites = new Dictionary<ulong, List<TrackedInvite>>();
	}

	public void RegisterEvents()
	{
		_client.GuildAvailable += GuildAvailableHandler;
		_client.GuildUpdated += GuildUpdatedHandler;
		_client.InviteCreated += InviteCreatedHandler;
		_client.UserJoined += GuildUserAddedHandler;
		_client.InviteDeleted += InviteDeletedHandler;
	}

	private Task InviteDeletedHandler(SocketGuildChannel channel, string tracker)
	{
		var invite = RemoveInvite(channel.Guild.Id, tracker).FirstOrDefault();

		if (invite == null)
			return Task.CompletedTask;

		_eventHandler.InviteDeletedEvent.Invoke(channel, invite);

		return Task.CompletedTask;
	}

	private async Task GuildUserAddedHandler(SocketGuildUser user)
	{
		using var scope = _serviceProvider.CreateScope();

		var translator = scope.ServiceProvider.GetRequiredService<Translation>();
		await translator.SetLanguage(user.Guild.Id);

		try
		{
			var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
				.GetGuildConfig(user.Guild.Id);

			if (user.IsBot)
				return;

			var newInvites = await FetchInvites(user.Guild);
			TrackedInvite usedInvite = null;

			try
			{
				usedInvite = GetUsedInvite(user.Guild.Id, newInvites);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get used invite.");
			}

			AddInvites(user.Guild.Id, newInvites);

			if (usedInvite != null)
			{
				UserInvite invite = new()
				{
					GuildId = user.Guild.Id,
					JoinedUserId = user.Id,
					JoinedAt = DateTime.UtcNow,
					InviteIssuerId = usedInvite.CreatorId,
					InviteCreatedAt = usedInvite.CreatedAt,
					TargetChannelId = usedInvite.TargetChannelId,
					UsedInvite = $"https://discord.gg/{usedInvite.Code}"
				};

				_logger.LogInformation(
					$"User {user.Username}#{user.Discriminator} joined guild {user.Guild.Name} with ID: {user.Guild.Id} using invite {usedInvite.Code}");

				if (guildConfig.ExecuteWhoIsOnJoin && !string.IsNullOrEmpty(guildConfig.ModInternalNotificationWebhook))
				{
					string message;

					if (invite.InviteIssuerId != 0 && invite.InviteCreatedAt != null)
						message = translator.Get<InviteNotificationTranslator>().NotificationAutoWhoisJoinWithAndFrom(user,
							invite.InviteIssuerId, invite.InviteCreatedAt.Value, user.CreatedAt.DateTime,
							invite.UsedInvite);
					else
						message = translator.Get<InviteNotificationTranslator>()
							.NotificationAutoWhoisJoinWith(user, user.CreatedAt.DateTime, invite.UsedInvite);

					await DiscordRest.ExecuteWebhook(guildConfig.ModInternalNotificationWebhook, null, message, AllowedMentions.None);
				}

				await scope.ServiceProvider.GetRequiredService<InviteRepository>().CreateInvite(invite);
			}
		}
		catch (ResourceNotFoundException)
		{
		}
	}

	private async Task<List<TrackedInvite>> FetchInvites(IGuild guild)
	{
		List<TrackedInvite> invites = new();

		try
		{
			var i = await guild.GetInvitesAsync();
			invites.AddRange(i.Select(x => new TrackedInvite(x, guild.Id)));
		}
		catch (HttpException)
		{
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Failed to get invites from guild {guild.Id}.");
		}

		try
		{
			var vanityInvite = await guild.GetVanityInviteAsync();
			invites.Add(new TrackedInvite(guild.Id, vanityInvite.Code, vanityInvite.Uses.GetValueOrDefault()));
		}
		catch (HttpException)
		{
		}

		return invites;
	}

	private async Task GuildAvailableHandler(SocketGuild guild)
	{
		AddInvites(guild.Id, await FetchInvites(guild));
	}

	private async Task GuildUpdatedHandler(SocketGuild _, SocketGuild newG)
	{
		IInviteMetadata invite = null;

		try
		{
			invite = await newG.GetVanityInviteAsync();
		}
		catch (HttpException)
		{
		}

		if (invite != null)
			AddInvite(invite.Guild.Id,
				new TrackedInvite(invite.Guild.Id, invite.Code, invite.Uses.GetValueOrDefault()));
	}

	private Task InviteCreatedHandler(SocketInvite invite)
	{
		AddInvite(invite.Guild.Id, new TrackedInvite(invite, invite.Guild.Id));

		return Task.CompletedTask;
	}

	private TrackedInvite GetUsedInvite(ulong guildId, List<TrackedInvite> currentInvites)
	{
		if (!_guildInvites.ContainsKey(guildId)) return null;

		var invites = _guildInvites[guildId];

		var changedInvites = invites.Where(x =>
			// Invite is in current invites and has new uses.
			currentInvites.Find(c => c.Code == x.Code) != null &&
			x.HasNewUses(currentInvites.Find(c => c.Code == x.Code)!.Uses) ||
			// Invite is not in current invites and has expired via max uses.
			x.MaxUses.GetValueOrDefault(0) - 1 == x.Uses && currentInvites.Find(c => c.Code == x.Code) == null
		).ToList();

		if (changedInvites.Count == 1)
			return changedInvites.First();

		var notExpiredInvites = changedInvites.Where(x => !x.IsExpired()).ToList();

		return notExpiredInvites.Count == 1 ? notExpiredInvites.First() : null;
	}

	private void AddInvites(ulong guildId, List<TrackedInvite> invites)
	{
		_guildInvites[guildId] = invites;
	}

	private void AddInvite(ulong guildId, TrackedInvite invite)
	{
		if (!_guildInvites.ContainsKey(guildId))
		{
			_guildInvites[guildId] = new List<TrackedInvite> { invite };
		}
		else
		{
			var invites = _guildInvites[guildId];
			var filteredInvites = invites.Where(x => x.Code != invite.Code).ToList();
			filteredInvites.Add(invite);
			_guildInvites[guildId] = filteredInvites;
		}
	}

	private IEnumerable<TrackedInvite> RemoveInvite(ulong guildId, string code)
	{
		if (!_guildInvites.ContainsKey(guildId)) return new List<TrackedInvite>();
		var invites = _guildInvites[guildId].TakeWhile(x => x.Code == code);
		_guildInvites[guildId].RemoveAll(x => x.Code == code);
		return invites.ToList();

	}
}