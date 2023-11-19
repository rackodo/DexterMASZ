using AutoMods.Enums;
using AutoMods.Events;
using AutoMods.Exceptions;
using AutoMods.Models;
using Bot.Abstractions;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Services;

namespace AutoMods.Data;

public class AutoModConfigRepository(DiscordRest discordRest, AutoModDatabase autoModDatabase,
    AutoModEventHandler eventHandler) : Repository(discordRest), IDeleteGuildData
{
    private readonly AutoModDatabase _autoModDatabase = autoModDatabase;
    private readonly AutoModEventHandler _eventHandler = eventHandler;

    public async Task DeleteGuildData(ulong guildId) =>
        await _autoModDatabase.DeleteAllPunishmentsConfigsForGuild(guildId);

    public async Task<List<AutoModConfig>> GetConfigsByGuild(ulong guildId) =>
        await _autoModDatabase.SelectAllPunishmentsConfigsForGuild(guildId);

    public async Task<AutoModConfig> GetConfigsByGuildAndType(ulong guildId, AutoModType type)
    {
        if (type == AutoModType.NoLinksAllowed)
            type = AutoModType.TooManyLinks;

        var config = await _autoModDatabase.SelectPunishmentsConfigForGuildAndType(guildId, type);

        return config ?? throw new ResourceNotFoundException($"Auto mod config {guildId}/{type} does not exist.");
    }

    public async Task<AutoModConfig> UpdateConfig(AutoModConfig newValue)
    {
        if (!Enum.IsDefined(typeof(AutoModType), newValue.AutoModType))
            throw new InvalidAutoModTypeException();

        if (!Enum.IsDefined(typeof(AutoModAction), newValue.AutoModAction))
            throw new InvalidAutoModActionException();

        var action = RestAction.Updated;
        AutoModConfig autoModConfig;

        try
        {
            autoModConfig = await GetConfigsByGuildAndType(newValue.GuildId, newValue.AutoModType);
        }
        catch (ResourceNotFoundException)
        {
            autoModConfig = new AutoModConfig();
            action = RestAction.Created;
        }

        autoModConfig.GuildId = newValue.GuildId;
        autoModConfig.AutoModType = newValue.AutoModType;
        autoModConfig.AutoModAction = newValue.AutoModAction;
        autoModConfig.PunishmentType = newValue.PunishmentType;
        autoModConfig.PunishmentDurationMinutes = newValue.PunishmentDurationMinutes;
        autoModConfig.IgnoreChannels = newValue.IgnoreChannels;
        autoModConfig.IgnoreRoles = newValue.IgnoreRoles;
        autoModConfig.TimeLimitMinutes = newValue.TimeLimitMinutes;
        autoModConfig.Limit = newValue.Limit;
        autoModConfig.CustomWordFilter = newValue.CustomWordFilter;
        autoModConfig.ChannelNotificationBehavior = newValue.ChannelNotificationBehavior;

        await _autoModDatabase.PutPunishmentsConfig(autoModConfig);

        if (action == RestAction.Created)
            _eventHandler.AutoModConfigCreatedEvent.Invoke(autoModConfig, Identity);
        else
            _eventHandler.AutoModConfigUpdatedEvent.Invoke(autoModConfig, Identity);

        return autoModConfig;
    }

    public async Task<AutoModConfig> DeleteConfigForGuild(ulong guildId, AutoModType type)
    {
        var config = await GetConfigsByGuildAndType(guildId, type);

        await _autoModDatabase.DeleteSpecificPunishmentsConfig(config);

        _eventHandler.AutoModConfigDeletedEvent.Invoke(config, Identity);

        return config;
    }
}
