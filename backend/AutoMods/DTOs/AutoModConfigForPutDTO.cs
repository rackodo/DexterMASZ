using System.ComponentModel.DataAnnotations;
using AutoMods.Enums;
using Punishments.Enums;

namespace AutoMods.DTOs;

public class AutoModConfigForPutDto
{
    [Required] public AutoModType AutoModType { get; set; }

    [Required] public AutoModAction AutoModAction { get; set; }

    public PunishmentType? PunishmentType { get; set; }
    public int? PunishmentDurationMinutes { get; set; }
    public ulong[] IgnoreChannels { get; set; } = Array.Empty<ulong>();
    public ulong[] IgnoreRoles { get; set; } = Array.Empty<ulong>();
    public int? TimeLimitMinutes { get; set; }
    public int? Limit { get; set; }
    public string CustomWordFilter { get; set; }
    public AutoModChannelNotificationBehavior ChannelNotificationBehavior { get; set; }
}