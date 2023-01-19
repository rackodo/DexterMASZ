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
    IAddAdminStats, IAddGuildStats, IAddSearch, IAddNetworks, IWhoIsResults, IDeleteGuildData
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

    public async Task AddAdminStatistics(dynamic adminStats) => adminStats.userMaps = await CountAllUserMaps();

    public async Task AddGuildStatistics(dynamic stats, ulong guildId) =>
        stats.userMaps = await CountAllUserMapsByGuild(guildId);

    public async Task AddNetworkData(dynamic network, List<string> modGuilds, ulong userId)
    {
        List<UserMapExpanded> userMaps = new();

        foreach (var userMap in (await GetUserMapsByUser(userId)).Where(userMap =>
                     modGuilds.Contains(userMap.GuildId.ToString())))
        {
            userMaps.Add(new UserMapExpanded(
                userMap,
                await _discordRest.FetchUserInfo(userMap.UserA, true),
                await _discordRest.FetchUserInfo(userMap.UserB, true),
                await _discordRest.FetchUserInfo(userMap.CreatorUserId, true)
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
            {
                userMapsViews.Add(new UserMapExpanded(
                    userMap,
                    await _discordRest.FetchUserInfo(userMap.UserA, true),
                    await _discordRest.FetchUserInfo(userMap.UserB, true),
                    await _discordRest.FetchUserInfo(userMap.CreatorUserId, true)
                ));
            }
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

    public async Task DeleteGuildData(ulong guildId) => await _userMapsDatabase.DeleteUserMapByGuild(guildId);

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
                userMapsInfo.Append("...");

            embed.AddField($"{translator.Get<UserMapTranslator>().UserMaps()} ({userMaps.Count})",
                userMapsInfo.ToString());
        }
    }

    public async Task<UserMap> GetUserMap(ulong guildId, ulong userA, ulong userB)
    {
        var userMaps = await _userMapsDatabase.GetUserMapByUserIdsAndGuildId(userA, userB, guildId);

        return userMaps ??
               throw new ResourceNotFoundException($"UserMap for guild {guildId} and users {userA}/{userB} not found.");
    }

    public async Task<UserMap> GetUserMap(int id)
    {
        var userMaps = await _userMapsDatabase.GetUserMapById(id);

        return userMaps ?? throw new ResourceNotFoundException($"UserMap for id {id} not found.");
    }

    public async Task<List<UserMap>> GetUserMapsByGuild(ulong guildId) =>
        await _userMapsDatabase.GetUserMapsByGuildId(guildId);

    public async Task<List<UserMap>> GetUserMapsByUser(ulong userId) =>
        await _userMapsDatabase.GetUserMapsByUserId(userId);

    public async Task<List<UserMap>> GetUserMapsByGuildAndUser(ulong guildId, ulong userId) =>
        await _userMapsDatabase.GetUserMapsByUserIdAndGuildId(userId, guildId);

    public async Task<UserMap> CreateOrUpdateUserMap(ulong guildId, ulong userA, ulong userB, string content)
    {
        if (await _discordRest.FetchUserInfo(userA, false) == null)
            throw new InvalidIUserException("User not found", userA);

        if (await _discordRest.FetchUserInfo(userB, false) == null)
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

    public async Task<int> CountAllUserMapsByGuild(ulong guildId) =>
        await _userMapsDatabase.CountUserMapsForGuild(guildId);

    public async Task<int> CountAllUserMaps() => await _userMapsDatabase.CountUserMaps();
}
