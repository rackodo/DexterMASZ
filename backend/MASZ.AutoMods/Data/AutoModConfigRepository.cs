using MASZ.AutoMods.Enums;
using MASZ.AutoMods.Events;
using MASZ.AutoMods.Exceptions;
using MASZ.AutoMods.Models;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;

namespace MASZ.AutoMods.Data;

public class AutoModConfigRepository : Repository
{
	private readonly AutoModDatabase _autoModDatabase;
	private readonly AutoModEventHandler _eventHandler;

	public AutoModConfigRepository(DiscordRest discordRest, AutoModDatabase autoModDatabase,
		AutoModEventHandler eventHandler) : base(discordRest)
	{
		_autoModDatabase = autoModDatabase;
		_eventHandler = eventHandler;
	}

	public async Task<List<AutoModConfig>> GetConfigsByGuild(ulong guildId)
	{
		return await _autoModDatabase.SelectAllPunishmentsConfigsForGuild(guildId);
	}

	public async Task<AutoModConfig> GetConfigsByGuildAndType(ulong guildId, AutoModType type)
	{
		var config = await _autoModDatabase.SelectPunishmentsConfigForGuildAndType(guildId, type);

		if (config == null)
			throw new ResourceNotFoundException($"AutoMod config {guildId}/{type} does not exist.");

		return config;
	}

	public async Task<AutoModConfig> UpdateConfig(AutoModConfig newValue)
	{
		if (!Enum.IsDefined(typeof(AutoModType), newValue.AutoModType))
			throw new InvalidAutoModTypeException();

		if (!Enum.IsDefined(typeof(AutoModAction), newValue.AutoModAction))
			throw new InvalidAutoModActionException();

		var action = RestAction.Updated;
		AutoModConfig autoModerationConfig;

		try
		{
			autoModerationConfig = await GetConfigsByGuildAndType(newValue.GuildId, newValue.AutoModType);
		}
		catch (ResourceNotFoundException)
		{
			autoModerationConfig = new AutoModConfig();
			action = RestAction.Created;
		}

		autoModerationConfig.GuildId = newValue.GuildId;
		autoModerationConfig.AutoModType = newValue.AutoModType;
		autoModerationConfig.AutoModAction = newValue.AutoModAction;
		autoModerationConfig.PunishmentType = newValue.PunishmentType;
		autoModerationConfig.PunishmentDurationMinutes = newValue.PunishmentDurationMinutes;
		autoModerationConfig.IgnoreChannels = newValue.IgnoreChannels;
		autoModerationConfig.IgnoreRoles = newValue.IgnoreRoles;
		autoModerationConfig.TimeLimitMinutes = newValue.TimeLimitMinutes;
		autoModerationConfig.Limit = newValue.Limit;
		autoModerationConfig.CustomWordFilter = newValue.CustomWordFilter;
		autoModerationConfig.SendDmNotification = newValue.SendDmNotification;
		autoModerationConfig.SendPublicNotification = newValue.SendPublicNotification;
		autoModerationConfig.ChannelNotificationBehavior = newValue.ChannelNotificationBehavior;

		await _autoModDatabase.PutPunishmentsConfig(autoModerationConfig);

		if (action == RestAction.Created)
			_eventHandler.AutoModConfigCreatedEvent.Invoke(autoModerationConfig, Identity);
		else
			_eventHandler.AutoModConfigUpdatedEvent.Invoke(autoModerationConfig, Identity);

		return autoModerationConfig;
	}
	
	public async Task<AutoModConfig> DeleteConfigForGuild(ulong guildId, AutoModType type)
	{
		var config = await GetConfigsByGuildAndType(guildId, type);

		await _autoModDatabase.DeleteSpecificPunishmentsConfig(config);

		_eventHandler.AutoModConfigDeletedEvent.Invoke(config, Identity);

		return config;
	}
}