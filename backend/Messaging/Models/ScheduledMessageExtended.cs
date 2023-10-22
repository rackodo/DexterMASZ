using Bot.Models;
using Discord;
using Messaging.Enums;

namespace Messaging.Models;

public class ScheduledMessageExtended(ScheduledMessage message, IUser creator, IUser lastEdited,
    IGuildChannel channel = null)
{
    public int Id { get; set; } = message.Id;
    public string Name { get; set; } = message.Name;
    public string Content { get; set; } = message.Content;
    public DateTime ScheduledFor { get; set; } = message.ScheduledFor;
    public ScheduledMessageStatus Status { get; set; } = message.Status;
    public ulong GuildId { get; set; } = message.GuildId;
    public ulong ChannelId { get; set; } = message.ChannelId;
    public ulong CreatorId { get; set; } = message.CreatorId;
    public ulong LastEditedById { get; set; } = message.LastEditedById;
    public DateTime CreatedAt { get; set; } = message.CreatedAt;
    public DateTime LastEditedAt { get; set; } = message.LastEditedAt;
    public ScheduledMessageFailureReason? FailureReason { get; set; } = message.FailureReason;

    public DiscordUser Creator { get; set; } = DiscordUser.GetDiscordUser(creator);
    public DiscordUser LastEdited { get; set; } = DiscordUser.GetDiscordUser(lastEdited);
    public DiscordChannel Channel { get; set; } = DiscordChannel.GetDiscordChannel(channel);
}
