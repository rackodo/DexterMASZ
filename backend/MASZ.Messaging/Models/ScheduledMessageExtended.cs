using Discord;
using MASZ.Bot.Models;
using MASZ.Messaging.Enums;

namespace MASZ.Messaging.Models;

public class ScheduledMessageExtended
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Content { get; set; }
	public DateTime ScheduledFor { get; set; }
	public ScheduledMessageStatus Status { get; set; }
	public ulong GuildId { get; set; }
	public ulong ChannelId { get; set; }
	public ulong CreatorId { get; set; }
	public ulong LastEditedById { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime LastEditedAt { get; set; }
	public ScheduledMessageFailureReason? FailureReason { get; set; }

	public DiscordUser Creator { get; set; }
	public DiscordUser LastEdited { get; set; }
	public DiscordChannel Channel { get; set; }

	public ScheduledMessageExtended(ScheduledMessage message, IUser creator, IUser lastEdited, IGuildChannel channel = null)
	{
		Id = message.Id;
		Name = message.Name;
		Content = message.Content;
		ScheduledFor = message.ScheduledFor;
		Status = message.Status;
		GuildId = message.GuildId;
		ChannelId = message.ChannelId;
		CreatorId = message.CreatorId;
		LastEditedById = message.LastEditedById;
		CreatedAt = message.CreatedAt;
		LastEditedAt = message.LastEditedAt;
		FailureReason = message.FailureReason;

		Creator = new DiscordUser(creator);
		LastEdited = new DiscordUser(lastEdited);
		Channel = new DiscordChannel(channel);
	}
}
