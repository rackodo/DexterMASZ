using Discord;

namespace Bot.Models;

public class DiscordChannel
{
	public ulong Id { get; set; }
	public string Name { get; set; }
	public int Position { get; set; }
	public int Type { get; set; }

	public static DiscordChannel GetDiscordChannel(IGuildChannel channel)
	{
		if (channel is null)
			return null;
		else if (channel.Id is 0)
			return null;
		else
			return new DiscordChannel(channel);
	}

	private DiscordChannel(IGuildChannel channel)
	{
		Id = channel.Id;
		Name = channel.Name;
		Position = channel.Position;
		Type = (int)channel.GetChannelType();
	}
}