using Bot.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bot.DTOs;

public class GuildConfigForPutDto
{
	[Required(ErrorMessage = "ModRoles field is required")]
	public ulong[] ModRoles { get; set; }

	[Required(ErrorMessage = "AdminRoles field is required")]
	public ulong[] AdminRoles { get; set; }

	[Required(ErrorMessage = "StaffChannels field is required")]
	public ulong[] StaffChannels { get; set; }

	[Required(ErrorMessage = "BotChannels field is required")]
	public ulong[] BotChannels { get; set; }

	public bool ModNotificationDm { get; set; }

	[Url(ErrorMessage = "Webhook needs to be a valid url")]
	[RegularExpression(@"https://(.*\.)?discord(app)?.com/api/webhooks/[0-9]+/.+", ErrorMessage = "Must be a valid webhook url")]
	public string AdminWebhook { get; set; }

	[Url(ErrorMessage = "Webhook needs to be a valid url")]
	[RegularExpression(@"https://(.*\.)?discord(app)?.com/api/webhooks/[0-9]+/.+", ErrorMessage = "Must be a valid webhook url")]
	public string StaffWebhook { get; set; }

	[Required(ErrorMessage = "ExecuteWhoIsOnJoin field is required")]
	public bool ExecuteWhoIsOnJoin { get; set; }

	[Required(ErrorMessage = "StrictModPermissionCheck field is required")]
	public bool StrictModPermissionCheck { get; set; }

	[Required(ErrorMessage = "PublishModeratorInfo field is required")]
	public bool PublishModeratorInfo { get; set; }

	public Language PreferredLanguage { get; set; } = Language.En;
}