using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.GuildAudits.Enums;
using MASZ.GuildAudits.Events;
using MASZ.GuildAudits.Exceptions;
using MASZ.GuildAudits.Models;

namespace MASZ.GuildAudits.Data;

public class GuildAuditConfigRepository : Repository
{
	private readonly GuildAuditEventHandler _eventHandler;
	private readonly GuildAuditDatabase _guildAuditDatabase;

	public GuildAuditConfigRepository(DiscordRest discordRest, GuildAuditDatabase guildAuditDatabase,
		GuildAuditEventHandler eventHandler) : base(discordRest)
	{
		_guildAuditDatabase = guildAuditDatabase;
		_eventHandler = eventHandler;
	}

	public async Task<List<GuildAuditConfig>> GetConfigsByGuild(ulong guildId)
	{
		return await _guildAuditDatabase.SelectAllAuditLogConfigsForGuild(guildId);
	}

	public async Task<GuildAuditConfig> GetConfigsByGuildAndType(ulong guildId, GuildAuditEvent type)
	{
		var config = await _guildAuditDatabase.SelectAuditLogConfigForGuildAndType(guildId, type);

		if (config == null)
			throw new ResourceNotFoundException($"GuildAuditLog config {guildId}/{type} does not exist.");

		return config;
	}

	public async Task<GuildAuditConfig> UpdateConfig(GuildAuditConfig newValue)
	{
		if (!Enum.IsDefined(typeof(GuildAuditEvent), newValue.GuildAuditLogEvent))
			throw new InvalidAuditLogEventException();

		var action = RestAction.Updated;
		GuildAuditConfig auditLogConfig;

		try
		{
			auditLogConfig = await GetConfigsByGuildAndType(newValue.GuildId, newValue.GuildAuditLogEvent);
		}
		catch (ResourceNotFoundException)
		{
			auditLogConfig = new GuildAuditConfig();
			action = RestAction.Created;
		}

		auditLogConfig.GuildId = newValue.GuildId;
		auditLogConfig.GuildAuditLogEvent = newValue.GuildAuditLogEvent;
		auditLogConfig.ChannelId = newValue.ChannelId;
		auditLogConfig.PingRoles = newValue.PingRoles;

		await _guildAuditDatabase.PutAuditLogConfig(auditLogConfig);

		if (action == RestAction.Created)
			_eventHandler.GuildAuditConfigCreatedEvent.Invoke(auditLogConfig, Identity);
		else
			_eventHandler.GuildAuditUpdatedEvent.Invoke(auditLogConfig, Identity);

		return auditLogConfig;
	}
	
	public async Task<GuildAuditConfig> DeleteConfigForGuild(ulong guildId, GuildAuditEvent type)
	{
		var config = await GetConfigsByGuildAndType(guildId, type);

		await _guildAuditDatabase.DeleteSpecificAuditLogConfig(config);

		_eventHandler.GuildAuditConfigDeletedEvent.Invoke(config, Identity);

		return config;
	}
}