using GuildAudits.DTOs;
using GuildAudits.Enums;
using System.ComponentModel.DataAnnotations;

namespace GuildAudits.Models;

public class GuildAuditConfig
{
	public GuildAuditConfig()
	{
	}

	public GuildAuditConfig(GuildAuditConfigForPutDto dto, ulong guildId)
	{
		GuildId = guildId;
		GuildAuditEvent = dto.GuildAuditEvent;
		ChannelId = dto.ChannelId;
		PingRoles = dto.PingRoles;
	}

	[Key] public int Id { get; set; }
	public ulong GuildId { get; set; }
	public GuildAuditEvent GuildAuditEvent { get; set; }
	public ulong ChannelId { get; set; }
	public ulong[] PingRoles { get; set; }
}