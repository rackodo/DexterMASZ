using Bot.Abstractions;
using Bot.Services;
using Discord;
using Levels.Events;
using Levels.Models;

namespace Levels.Data;

public class UserRankcardConfigRepository : Repository
{
    private readonly LevelsDatabase _database;
    private readonly LevelsEventHandler _eventHandler;

    public UserRankcardConfigRepository(DiscordRest discordRest, LevelsDatabase database,
        LevelsEventHandler eventHandler) : base(discordRest)
    {
        _database = database;
        _eventHandler = eventHandler;
    }

    public UserRankcardConfig? GetRankcard(ulong userid) => _database.GetUserRankcardConfig(userid);

    public UserRankcardConfig GetOrDefaultRankcard(IUser user) => GetOrDefaultRankcard(user.Id);

    public UserRankcardConfig GetOrDefaultRankcard(ulong userid)
    {
        var config = _database.GetUserRankcardConfig(userid);
        return config is null ? new UserRankcardConfig(userid) : config;
    }

    public async Task<UserRankcardConfig> GetOrCreateRankcard(IUser user) => await GetOrCreateRankcard(user.Id);

    public async Task<UserRankcardConfig> GetOrCreateRankcard(ulong userid)
    {
        var config = _database.GetUserRankcardConfig(userid);
        if (config is null)
        {
            config = new UserRankcardConfig(userid);
            await _database.RegisterUserRankcardConfig(config);
        }

        return config;
    }

    public async Task RegisterRankcard(UserRankcardConfig userRankcardConfig)
    {
        try
        {
            await _database.RegisterUserRankcardConfig(userRankcardConfig);
        }
        catch (Exception)
        {
            await UpdateRankcard(userRankcardConfig);
        }
    }

    public async Task UpdateRankcard(UserRankcardConfig userRankcardConfig) =>
        await _database.UpdateUserRankcardConfig(userRankcardConfig);

    public async Task DeleteRankcard(ulong userid)
    {
        var card = GetRankcard(userid);
        if (card is null) return;
        await DeleteRankcard(card);
    }

    public async Task DeleteRankcard(UserRankcardConfig userRankcardConfig) =>
        await _database.DeleteUserRankcardConfig(userRankcardConfig);
}
