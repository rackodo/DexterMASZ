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

public class AutoModConfigRepository : Repository, DeleteGuildData
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
			throw new ResourceNotFoundException($"Automod config {guildId}/{type} does not exist.");

		return config;
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

	public async Task DeleteGuildData(ulong guildId)
	{
		await _autoModDatabase.DeleteAllPunishmentsConfigsForGuild(guildId);
	}
}