using Bot.Abstractions;
using Bot.Services;
using Discord;
using Levels.Events;
using Levels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Data;

public class UserRankcardConfigRepository : Repository
{
	private DiscordRest _discordRest;
	private LevelsDatabase _database;
	private LevelsEventHandler _eventHandler;

	public UserRankcardConfigRepository(DiscordRest discordRest, LevelsDatabase database, LevelsEventHandler eventHandler) : base(discordRest)
	{
		_discordRest = discordRest;
		_database = database;
		_eventHandler = eventHandler;
	}

	public UserRankcardConfig? GetRankcard(ulong userid) => _database.GetUserRankcardConfig(userid);

	public UserRankcardConfig GetOrDefaultRankcard(IUser user) => GetOrDefaultRankcard(user.Id);
	public UserRankcardConfig GetOrDefaultRankcard(ulong userid)
	{
		UserRankcardConfig? config = _database.GetUserRankcardConfig(userid);
		if (config is null)
			return new UserRankcardConfig(userid);
		return config;
	}

	public async Task<UserRankcardConfig> GetOrCreateRankcard(IUser user)
	{
		return await GetOrCreateRankcard(user.Id);
	}

	public async Task<UserRankcardConfig> GetOrCreateRankcard(ulong userid)
	{
		UserRankcardConfig? config = _database.GetUserRankcardConfig(userid);
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

	public async Task UpdateRankcard(UserRankcardConfig userRankcardConfig)
	{
		await _database.UpdateUserRankcardConfig(userRankcardConfig);
	}

	public async Task DeleteRankcard(ulong userid)
	{
		var card = GetRankcard(userid);
		if (card is null) return;
		await DeleteRankcard(card);
	}

	public async Task DeleteRankcard(UserRankcardConfig userRankcardConfig)
	{
		await _database.DeleteUserRankcardConfig(userRankcardConfig);
	}

}
