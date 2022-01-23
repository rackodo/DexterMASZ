using Discord;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;

namespace MASZ.Bot.Views;

public class DiscordChannelView
{
	public DiscordChannelView(IGuildChannel channel)
	{
		if (channel is null)
			throw new ResourceNotFoundException("Channel for DiscordChannelView is equal to null!");

		if (channel.Id is 0)
			throw new ResourceNotFoundException("Channel for DiscordChannelView has an ID of 0!");

		Id = channel.Id.ToString();
		Name = channel.Name;
		Position = channel.Position;
		Type = (int)channel.GetChannelType();
	}

	public string Id { get; set; }
	public string Name { get; set; }
	public int Position { get; set; }
	public int Type { get; set; }
}