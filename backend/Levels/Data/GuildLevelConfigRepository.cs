using Bot.Abstractions;
using Bot.Extensions;
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

public class GuildLevelConfigRepository : Repository
{
	private DiscordRest _discordRest;
	private LevelsDatabase _database;
	private LevelsEventHandler _eventHandler;

	public GuildLevelConfigRepository(DiscordRest discordRest, LevelsDatabase database, LevelsEventHandler eventHandler) : base(discordRest)
	{
		_discordRest = discordRest;
		_database = database;
		_eventHandler = eventHandler;
	}

	public async Task<GuildLevelConfig> GetOrCreateConfig(IGuild guild)
	{
		return await GetOrCreateConfig(guild.Id);
	}

	public async Task<GuildLevelConfig> GetOrCreateConfig(ulong guildId)
	{
		GuildLevelConfig? config = _database.GetGuildLevelConfig(guildId);
		bool created = false;
		if (config is null)
		{
			created = true;
			config = new GuildLevelConfig(guildId);
			await _database.RegisterGuildLevelConfig(config);
		}

		if (created)
			_eventHandler.GuildLevelConfigCreatedEvent.Invoke(config);

		return config;
	}

	public GuildLevelConfig? GetConfig(ulong guildid)
	{
		return _database.GetGuildLevelConfig(guildid);
	}

	public GuildLevelConfig[] GetAllRegistered()
	{
		return _database.GetAllGuildLevelConfigs();
	}

	public async Task UpdateConfig(GuildLevelConfig guildLevelConfig)
	{
		_eventHandler.GuildLevelConfigCreatedEvent.Invoke(guildLevelConfig);
		await _database.UpdateGuildLevelConfig(guildLevelConfig);
	}

	public async Task DeleteConfig(GuildLevelConfig guildLevelConfig)
	{
		_eventHandler.GuildLevelConfigDeletedEvent.Invoke(guildLevelConfig);
		await _database.DeleteGuildLevelConfig(guildLevelConfig);
	}

}
