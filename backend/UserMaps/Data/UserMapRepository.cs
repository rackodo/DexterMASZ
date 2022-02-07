using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;
using Discord;
using Humanizer;
using System.Text;
using UserMaps.Events;
using UserMaps.Exceptions;
using UserMaps.Models;
using UserMaps.Translators;
using Utilities.Dynamics;

namespace UserMaps.Data;

public class UserMapRepository : Repository,
	AddAdminStats, CacheUsers, AddGuildStats, AddSearch, AddNetworks, WhoIsResults, DeleteGuildData
{
	private readonly DiscordRest _discordRest;
	private readonly UserMapEventHandler _eventHandler;
	private readonly UserMapDatabase _userMapsDatabase;

	public UserMapRepository(DiscordRest discordRest, UserMapDatabase userMapsDatabase,
		UserMapEventHandler eventHandler) : base(discordRest)
	{
		_userMapsDatabase = userMapsDatabase;
		_discordRest = discordRest;
		_eventHandler = eventHandler;
	}

	public async Task DeleteGuildData(ulong guildId)
	{
		await _userMapsDatabase.DeleteUserMapByGuild(guildId);
	}

	public async Task AddAdminStatistics(dynamic adminStats)
	{
		adminStats.userMaps = await CountAllUserMaps();
	}

	public async Task AddGuildStatistics(dynamic stats, ulong guildId)
	{
		stats.userMaps = await CountAllUserMapsByGuild(guildId);
	}

	public async Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId)
	{
		List<UserMapExpanded> userMaps = new();

		foreach (var userMap in (await GetUserMapsByUser(userId)).Where(userMap => modGuilds.Contains(userMap.GuildId.ToString())))
		{
			userMaps.Add(new UserMapExpanded(
				userMap,
				await _discordRest.FetchUserInfo(userMap.UserA, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(userMap.UserB, CacheBehavior.OnlyCache),
				await _discordRest.FetchUserInfo(userMap.CreatorUserId, CacheBehavior.OnlyCache)
			));
		}

		network.userMaps = userMaps;
	}

	public async Task AddSearchData(dynamic data, ulong guildId, string search)
	{
		List<UserMapExpanded> userMapsViews = new();
		try
		{
			var userId = ulong.Parse(search);
			var userMaps = await GetUserMapsByGuildAndUser(guildId, userId);

			foreach (var userMap in userMaps)
				userMapsViews.Add(new UserMapExpanded(
					userMap,
					await _discordRest.FetchUserInfo(userMap.UserA, CacheBehavior.OnlyCache),
					await _discordRest.FetchUserInfo(userMap.UserB, CacheBehavior.OnlyCache),
					await _discordRest.FetchUserInfo(userMap.CreatorUserId, CacheBehavior.OnlyCache)
				));
		}
		catch (ResourceNotFoundException)
		{
		}
		catch (FormatException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (OverflowException)
		{
		}

		data.userMapsViews = userMapsViews;
	}

	public async Task CacheKnownUsers(List<ulong> handledUsers)
	{
		foreach (var userMaps in await _userMapsDatabase.SelectLatestUserMaps(DateTime.UtcNow.AddYears(-3), 100))
		{
			if (!handledUsers.Contains(userMaps.UserA))
			{
				await _discordRest.FetchUserInfo(userMaps.UserA, CacheBehavior.IgnoreCache);
				handledUsers.Add(userMaps.UserA);
			}

			if (!handledUsers.Contains(userMaps.UserB))
			{
				await _discordRest.FetchUserInfo(userMaps.UserB, CacheBehavior.IgnoreCache);
				handledUsers.Add(userMaps.UserB);
			}

			if (handledUsers.Contains(userMaps.CreatorUserId)) continue;

			await _discordRest.FetchUserInfo(userMaps.CreatorUserId, CacheBehavior.IgnoreCache);
			handledUsers.Add(userMaps.CreatorUserId);
		}
	}

	public async Task AddWhoIsInformation(EmbedBuilder embed, IGuildUser user, IInteractionContext context,
		Translation translator)
	{
		var userMaps = await GetUserMapsByGuildAndUser(context.Guild.Id, user.Id);

		if (userMaps.Count > 0)
		{
			StringBuilder userMapsInfo = new();

			foreach (var userMap in userMaps.Take(5))
			{
				var otherUser = userMap.UserA == user.Id ? userMap.UserB : userMap.UserA;
				userMapsInfo.AppendLine($"<@{otherUser}> - {userMap.Reason.Truncate(80)}");
			}

			if (userMaps.Count > 5)
				userMapsInfo.Append("[...]");

			embed.AddField($"{translator.Get<UserMapTranslator>().UserMaps()} [{userMaps.Count}]",
				userMapsInfo.ToString());
		}
	}

	public async Task<UserMap> GetUserMap(ulong guildId, ulong userA, ulong userB)
	{
		var userMaps = await _userMapsDatabase.GetUserMapByUserIdsAndGuildId(userA, userB, guildId);

		if (userMaps == null)
			throw new ResourceNotFoundException($"UserMap for guild {guildId} and users {userA}/{userB} not found.");

		return userMaps;
	}

	public async Task<UserMap> GetUserMap(int id)
	{
		var userMaps = await _userMapsDatabase.GetUserMapById(id);

		if (userMaps == null)
			throw new ResourceNotFoundException($"UserMap for id {id} not found.");

		return userMaps;
	}

	public async Task<List<UserMap>> GetUserMapsByGuild(ulong guildId)
	{
		return await _userMapsDatabase.GetUserMapsByGuildId(guildId);
	}

	public async Task<List<UserMap>> GetUserMapsByUser(ulong userId)
	{
		return await _userMapsDatabase.GetUserMapsByUserId(userId);
	}

	public async Task<List<UserMap>> GetUserMapsByGuildAndUser(ulong guildId, ulong userId)
	{
		return await _userMapsDatabase.GetUserMapsByUserIdAndGuildId(userId, guildId);
	}

	public async Task<UserMap> CreateOrUpdateUserMap(ulong guildId, ulong userA, ulong userB, string content)
	{
		if (await _discordRest.FetchUserInfo(userA, CacheBehavior.Default) == null)
			throw new InvalidIUserException("User not found", userA);

		if (await _discordRest.FetchUserInfo(userB, CacheBehavior.Default) == null)
			throw new InvalidIUserException("User not found", userB);

		if (userA == userB)
			throw new InvalidUserMapException();

		UserMap userMaps;
		var action = RestAction.Updated;

		try
		{
			userMaps = await GetUserMap(guildId, userA, userB);
		}
		catch (ResourceNotFoundException)
		{
			userMaps = new UserMap
			{
				GuildId = guildId,
				UserA = userA,
				UserB = userB
			};
			action = RestAction.Created;
		}

		userMaps.CreatedAt = DateTime.UtcNow;
		userMaps.CreatorUserId = Identity.Id;

		userMaps.Reason = content;

		await _userMapsDatabase.SaveUserMap(userMaps);

		if (action == RestAction.Created)
			_eventHandler.UserMapUpdatedEvent.Invoke(userMaps, Identity);
		else
			_eventHandler.UserMapUpdatedEvent.Invoke(userMaps, Identity);

		return userMaps;
	}

	public async Task DeleteUserMap(int id)
	{
		var userMaps = await GetUserMap(id);

		await _userMapsDatabase.DeleteUserMap(userMaps);

		_eventHandler.UserMapDeletedEvent.Invoke(userMaps, Identity);
	}

	public async Task<int> CountAllUserMapsByGuild(ulong guildId)
	{
		return await _userMapsDatabase.CountUserMapsForGuild(guildId);
	}

	public async Task<int> CountAllUserMaps()
	{
		return await _userMapsDatabase.CountUserMaps();
	}
}