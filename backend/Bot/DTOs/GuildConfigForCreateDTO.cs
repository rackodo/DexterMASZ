using Bot.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bot.DTOs;

public class GuildConfigForCreateDto
{
	[Required(ErrorMessage = "GuildId field is required")]
	public ulong GuildId { get; set; }

	[Required(ErrorMessage = "ModRoles field is required")]
	public ulong[] ModRoles { get; set; }

	[Required(ErrorMessage = "AdminRoles field is required")]
	public ulong[] AdminRoles { get; set; }

	[Required(ErrorMessage = "StaffChannels field is required")]
	public ulong[] StaffChannels { get; set; }

	[Required(ErrorMessage = "BotChannels field is required")]
	public ulong[] BotChannels { get; set; }

	[Required(ErrorMessage = "ModNotificationDm field is required")]
	public bool ModNotificationDm { get; set; }

	[Required(ErrorMessage = "StaffLogs field is required")]
	public ulong StaffLogs { get; set; }

	[Required(ErrorMessage = "StaffAnnouncements field is required")]
	public ulong StaffAnnouncements { get; set; }

	[Required(ErrorMessage = "ExecuteWhoIsOnJoin field is required")]
	public bool ExecuteWhoIsOnJoin { get; set; }

	[Required(ErrorMessage = "StrictModPermissionCheck field is required")]
	public bool StrictModPermissionCheck { get; set; }

	[Required(ErrorMessage = "PublishModeratorInfo field is required")]
	public bool PublishModeratorInfo { get; set; }

	public Language? PreferredLanguage { get; set; } = null;
}