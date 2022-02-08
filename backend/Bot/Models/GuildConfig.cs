using Bot.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bot.Models;

public class GuildConfig
{
	[Key] public ulong GuildId { get; set; }
	public ulong[] ModRoles { get; set; }
	public ulong[] AdminRoles { get; set; }
	public ulong[] StaffChannels { get; set; }
	public bool ModNotificationDm { get; set; }
	public string ModNotificationWebhook { get; set; }
	public bool StrictModPermissionCheck { get; set; }
	public bool ExecuteWhoIsOnJoin { get; set; }
	public bool PublishModeratorInfo { get; set; } = true;
	public Language PreferredLanguage { get; set; }
}