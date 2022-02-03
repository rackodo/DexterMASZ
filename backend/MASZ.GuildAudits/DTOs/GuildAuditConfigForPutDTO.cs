using MASZ.GuildAudits.Enums;
using System.ComponentModel.DataAnnotations;

namespace MASZ.GuildAudits.DTOs;

public class GuildAuditConfigForPutDto
{
	[Required] public GuildAuditEvent GuildAuditEvent { get; set; }
	public ulong ChannelId { get; set; }
	public ulong[] PingRoles { get; set; }
}