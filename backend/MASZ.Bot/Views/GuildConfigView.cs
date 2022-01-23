using MASZ.Bot.Enums;
using MASZ.Bot.Models;

namespace MASZ.Bot.Views;

public class GuildConfigView
{
	public GuildConfigView(GuildConfig config)
	{
		Id = config.Id;
		GuildId = config.GuildId.ToString();
		ModRoles = config.ModRoles.Select(x => x.ToString()).ToArray();
		AdminRoles = config.AdminRoles.Select(x => x.ToString()).ToArray();
		MutedRoles = config.MutedRoles.Select(x => x.ToString()).ToArray();
		ModNotificationDm = config.ModNotificationDm;
		ModPublicNotificationWebhook = config.ModPublicNotificationWebhook;
		ModInternalNotificationWebhook = config.ModInternalNotificationWebhook;
		StrictModPermissionCheck = config.StrictModPermissionCheck;
		ExecuteWhoIsOnJoin = config.ExecuteWhoIsOnJoin;
		PublishModeratorInfo = config.PublishModeratorInfo;
		PreferredLanguage = config.PreferredLanguage;
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public string[] ModRoles { get; set; }
	public string[] AdminRoles { get; set; }
	public string[] MutedRoles { get; set; }
	public bool ModNotificationDm { get; set; }
	public string ModPublicNotificationWebhook { get; set; }
	public string ModInternalNotificationWebhook { get; set; }
	public bool StrictModPermissionCheck { get; set; }
	public bool ExecuteWhoIsOnJoin { get; set; }
	public bool PublishModeratorInfo { get; set; }
	public Language PreferredLanguage { get; set; }
}