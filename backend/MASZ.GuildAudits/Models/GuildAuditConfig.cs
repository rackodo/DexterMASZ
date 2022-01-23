using System.ComponentModel.DataAnnotations;
using MASZ.GuildAudits.DTOs;
using MASZ.GuildAudits.Enums;

namespace MASZ.GuildAudits.Models;

public class GuildAuditConfig
{
	public GuildAuditConfig()
	{
	}

	public GuildAuditConfig(GuildAuditConfigForPutDto dto, ulong guildId)
	{
		GuildId = guildId;
		GuildAuditLogEvent = dto.GuildAuditLogEvent;
		ChannelId = dto.ChannelId;
		PingRoles = dto.PingRoles;
	}

	[Key] public int Id { get; set; }
	public ulong GuildId { get; set; }
	public GuildAuditEvent GuildAuditLogEvent { get; set; }
	public ulong ChannelId { get; set; }
	public ulong[] PingRoles { get; set; }
}