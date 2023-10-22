using Bot.Abstractions;
using Bot.Extensions;
using Bot.Services;
using Discord;
using Levels.Events;
using Levels.Models;

namespace Levels.Data;

public class GuildLevelConfigRepository(DiscordRest discordRest, LevelsDatabase database, LevelsEventHandler eventHandler) : Repository(discordRest)
{
    private readonly LevelsDatabase _database = database;
    private readonly LevelsEventHandler _eventHandler = eventHandler;

    public async Task<GuildLevelConfig> GetOrCreateConfig(IGuild guild) => await GetOrCreateConfig(guild.Id);

    public async Task<GuildLevelConfig> GetOrCreateConfig(ulong guildId)
    {
        var config = _database.GetGuildLevelConfig(guildId);
        var created = false;
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

    public GuildLevelConfig GetConfig(ulong guildid) => _database.GetGuildLevelConfig(guildid);

    public GuildLevelConfig[] GetAllRegistered() => _database.GetAllGuildLevelConfigs();

    public async Task UpdateConfig(GuildLevelConfig guildLevelConfig)
    {
        _eventHandler.GuildLevelConfigCreatedEvent.Invoke(guildLevelConfig);
        await _database.UpdateGuildLevelConfig();
    }

    public async Task DeleteConfig(GuildLevelConfig guildLevelConfig)
    {
        _eventHandler.GuildLevelConfigDeletedEvent.Invoke(guildLevelConfig);
        await _database.DeleteGuildLevelConfig(guildLevelConfig);
    }
}
