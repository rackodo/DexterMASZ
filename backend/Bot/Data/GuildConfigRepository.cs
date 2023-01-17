using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Events;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;

namespace Bot.Data;

public class GuildConfigRepository : Repository, IAddAdminStats
{
    private readonly BotDatabase _context;
    private readonly DiscordRest _discordRest;
    private readonly BotEventHandler _eventHandler;

    public GuildConfigRepository(BotDatabase context, DiscordRest discordRest, BotEventHandler eventHandler) :
        base(discordRest)
    {
        _context = context;
        _discordRest = discordRest;
        _eventHandler = eventHandler;
    }

    public async Task AddAdminStatistics(dynamic adminStats) => adminStats.guilds = await CountGuildConfigs();

    public async Task RequireGuildRegistered(ulong guildId) => await GetGuildConfig(guildId);

    public async Task<GuildConfig> GetGuildConfig(ulong guildId)
    {
        var guildConfig = await _context.SelectSpecificGuildConfig(guildId);

        return guildConfig is null
            ? throw new UnregisteredGuildException($"GuildConfig with id {guildId} not found.", guildId)
            : guildConfig;
    }

    public async Task<List<GuildConfig>> GetAllGuildConfigs() => await _context.SelectAllGuildConfigs();

    public async Task<int> CountGuildConfigs() => await _context.CountAllGuildConfigs();

    public async Task<GuildConfig> CreateGuildConfig(GuildConfig guildConfig, bool importExistingBans)
    {
        var guild = _discordRest.FetchGuildInfo(guildConfig.GuildId, CacheBehavior.IgnoreCache);

        if (guild is null)
            throw new ResourceNotFoundException($"Guild with id {guildConfig.GuildId} not found.");

        foreach (var role in guildConfig.ModRoles)
        {
            if (guild.Roles.All(r => r.Id != role))
                throw new RoleNotFoundException(role);
        }

        foreach (var role in guildConfig.AdminRoles)
        {
            if (guild.Roles.All(r => r.Id != role))
                throw new RoleNotFoundException(role);
        }

        await _context.SaveGuildConfig(guildConfig);

        _eventHandler.GuildRegisteredEvent.Invoke(guildConfig, importExistingBans);

        return guildConfig;
    }

    public async Task<GuildConfig> UpdateGuildConfig(GuildConfig guildConfig)
    {
        var guild = _discordRest.FetchGuildInfo(guildConfig.GuildId, CacheBehavior.IgnoreCache);

        if (guild is null)
            throw new ResourceNotFoundException($"Guild with id {guildConfig.GuildId} not found.");

        foreach (var role in guildConfig.ModRoles)
        {
            if (guild.Roles.All(r => r.Id != role))
                throw new RoleNotFoundException(role);
        }

        foreach (var role in guildConfig.AdminRoles)
        {
            if (guild.Roles.All(r => r.Id != role))
                throw new RoleNotFoundException(role);
        }

        await _context.InternalUpdateGuildConfig(guildConfig);

        _eventHandler.GuildUpdatedEvent.Invoke(guildConfig);

        return guildConfig;
    }

    public async Task<GuildConfig> DeleteGuildConfig(ulong guildId)
    {
        var guildConfig = await GetGuildConfig(guildId);

        await _context.DeleteSpecificGuildConfig(guildConfig);

        _eventHandler.GuildDeletedEvent.Invoke(guildConfig);

        return guildConfig;
    }
}
