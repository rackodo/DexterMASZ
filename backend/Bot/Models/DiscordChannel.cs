using Bot.Exceptions;
using Discord;

namespace Bot.Models;

public class DiscordChannel
{
	public static DiscordChannel GetDiscordChannel(IGuildChannel channel)
	{
		if (channel is null)
			return null;
		else if (channel.Id is 0)
			return null;
		else
			return DiscordChannel.GetDiscordChannel(channel);
	}

	private DiscordChannel(IGuildChannel channel)
	{
		Id = channel.Id;
		Name = channel.Name;
		Position = channel.Position;
		Type = (int)channel.GetChannelType();
	}

	public ulong Id { get; set; }
	public string Name { get; set; }
	public int Position { get; set; }
	public int Type { get; set; }
}