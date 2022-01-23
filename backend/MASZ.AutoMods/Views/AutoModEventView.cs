using MASZ.AutoMods.Enums;
using MASZ.AutoMods.Models;

namespace MASZ.AutoMods.Views;

public class AutoModEventView
{
	public AutoModEventView(AutoModEvent autoModerationEvent)
	{
		Id = autoModerationEvent.Id;
		GuildId = autoModerationEvent.GuildId.ToString();
		AutoModerationType = autoModerationEvent.AutoModType;
		AutoModerationAction = autoModerationEvent.AutoModAction;
		UserId = autoModerationEvent.UserId.ToString();
		Username = autoModerationEvent.Username;
		Nickname = autoModerationEvent.Nickname;
		Discriminator = autoModerationEvent.Discriminator;
		MessageId = autoModerationEvent.MessageId.ToString();
		MessageContent = autoModerationEvent.MessageContent;
		CreatedAt = autoModerationEvent.CreatedAt;
		AssociatedCaseId = autoModerationEvent.AssociatedCaseId;
	}

	public int Id { get; set; }
	public string GuildId { get; set; }
	public AutoModType AutoModerationType { get; set; }
	public AutoModAction AutoModerationAction { get; set; }
	public string UserId { get; set; }
	public string Username { get; set; }
	public string Nickname { get; set; }
	public string Discriminator { get; set; }
	public string MessageId { get; set; }
	public string MessageContent { get; set; }
	public DateTime CreatedAt { get; set; }
	public int? AssociatedCaseId { get; set; }
}