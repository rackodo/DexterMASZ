using System.ComponentModel.DataAnnotations;
using MASZ.GuildAudits.Enums;

namespace MASZ.GuildAudits.DTOs;

public class GuildAuditConfigForPutDto
{
	[Required] public GuildAuditEvent GuildAuditEvent { get; set; }
	public ulong ChannelId { get; set; }
	public ulong[] PingRoles { get; set; }
}