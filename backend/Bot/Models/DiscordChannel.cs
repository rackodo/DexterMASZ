using Discord;
using Bot.Exceptions;
using Bot.Extensions;

namespace Bot.Models;

public class DiscordChannel
{
	public DiscordChannel(IGuildChannel channel)
	{
		if (channel is null)
			throw new ResourceNotFoundException("Channel for DiscordChannelView is equal to null!");

		if (channel.Id is 0)
			throw new ResourceNotFoundException("Channel for DiscordChannelView has an ID of 0!");

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