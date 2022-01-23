using System.ComponentModel.DataAnnotations;
using MASZ.AutoMods.Enums;
using MASZ.Punishments.Enums;

namespace MASZ.AutoMods.DTOs;

public class AutoModConfigForPutDto
{
	[Required] public AutoModType AutoModerationType { get; set; }

	[Required] public AutoModAction AutoModerationAction { get; set; }

	public PunishmentType? PunishmentType { get; set; }
	public int? PunishmentDurationMinutes { get; set; }
	public ulong[] IgnoreChannels { get; set; } = Array.Empty<ulong>();
	public ulong[] IgnoreRoles { get; set; } = Array.Empty<ulong>();
	public int? TimeLimitMinutes { get; set; }
	public int? Limit { get; set; }
	public string CustomWordFilter { get; set; }
	public bool SendDmNotification { get; set; }
	public bool SendPublicNotification { get; set; }
	public AutoModChannelNotificationBehavior ChannelNotificationBehavior { get; set; }
}