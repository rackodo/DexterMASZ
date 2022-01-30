using Discord;
using MASZ.Bot.Models;

namespace MASZ.Messaging.Models;

public class ScheduledMessageExtended
{
	public ScheduledMessageExtended(ScheduledMessage scheduledMessage,
		IUser creator, IUser lastEdited, IGuildChannel channel = null)
	{
		ScheduledMessage = scheduledMessage;

		Creator = new DiscordUser(creator);
		LastEdited = new DiscordUser(lastEdited);
		Channel = new DiscordChannel(channel);
	}

	public ScheduledMessage ScheduledMessage { get; set; }
	public DiscordUser Creator { get; set; }
	public DiscordUser LastEdited { get; set; }
	public DiscordChannel Channel { get; set; }
}
