using AutoMods.DTOs;
using AutoMods.Enums;
using Punishments.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoMods.Models;

public class AutoModConfig
{
    [Key] public int Id { get; set; }
    public ulong GuildId { get; set; }
    public AutoModType AutoModType { get; set; }
    public AutoModAction AutoModAction { get; set; }
    public PunishmentType? PunishmentType { get; set; }
    public int? PunishmentDurationMinutes { get; set; }
    public ulong[] IgnoreChannels { get; set; }
    public ulong[] IgnoreRoles { get; set; }
    public int? TimeLimitMinutes { get; set; }
    public int? Limit { get; set; }
    public string CustomWordFilter { get; set; }
    public AutoModChannelNotificationBehavior ChannelNotificationBehavior { get; set; }
    public bool SendDmNotification { get; set; }

    public AutoModConfig()
    {
    }

    public AutoModConfig(AutoModConfigForPutDto dto, ulong guildId)
    {
        GuildId = guildId;
        AutoModType = dto.AutoModType;
        AutoModAction = dto.AutoModAction;
        PunishmentType = dto.PunishmentType;
        PunishmentDurationMinutes = dto.PunishmentDurationMinutes;
        IgnoreChannels = dto.IgnoreChannels;
        IgnoreRoles = dto.IgnoreRoles;
        TimeLimitMinutes = dto.TimeLimitMinutes;
        Limit = dto.Limit;
        CustomWordFilter = dto.CustomWordFilter;
        ChannelNotificationBehavior = dto.ChannelNotificationBehavior;
        SendDmNotification = dto.SendDmNotification;
    }
}
