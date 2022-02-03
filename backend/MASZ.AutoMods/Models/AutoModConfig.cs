using MASZ.AutoMods.DTOs;
using MASZ.AutoMods.Enums;
using MASZ.Punishments.Enums;
using System.ComponentModel.DataAnnotations;

namespace MASZ.AutoMods.Models;

public class AutoModConfig
{
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
		SendDmNotification = dto.SendDmNotification;
		SendPublicNotification = dto.SendPublicNotification;
		ChannelNotificationBehavior = dto.ChannelNotificationBehavior;
	}

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
	public bool SendDmNotification { get; set; }
	public bool SendPublicNotification { get; set; }
	public AutoModChannelNotificationBehavior ChannelNotificationBehavior { get; set; }
}