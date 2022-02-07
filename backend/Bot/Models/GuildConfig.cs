using Bot.Enums;

namespace Bot.Models;

public class GuildConfig
{
	public int Id { get; set; }
	public ulong GuildId { get; set; }
	public ulong[] ModRoles { get; set; }
	public ulong[] AdminRoles { get; set; }
	public bool ModNotificationDm { get; set; }
	public string ModPublicNotificationWebhook { get; set; }
	public string ModInternalNotificationWebhook { get; set; }
	public bool StrictModPermissionCheck { get; set; }
	public bool ExecuteWhoIsOnJoin { get; set; }
	public bool PublishModeratorInfo { get; set; } = true;
	public Language PreferredLanguage { get; set; }
}