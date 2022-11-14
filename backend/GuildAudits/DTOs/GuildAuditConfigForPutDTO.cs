using System.ComponentModel.DataAnnotations;
using GuildAudits.Enums;

namespace GuildAudits.DTOs;

public class GuildAuditConfigForPutDto
{
    [Required] public GuildAuditLogEvent GuildAuditLogEvent { get; set; }

    [Required] public ulong ChannelId { get; set; }

    [Required] public ulong[] PingRoles { get; set; }

    [Required] public ulong[] IgnoreRoles { get; set; }

    [Required] public ulong[] IgnoreChannels { get; set; }
}