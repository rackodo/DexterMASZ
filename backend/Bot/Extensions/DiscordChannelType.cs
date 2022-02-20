using Discord;

namespace Bot.Extensions;

public static class DiscordChannelType
{
	public static ChannelType GetChannelType(this IChannel channel)
	{
		return channel.GetType().GetInterfaces() switch
		{
			{ } channelType when channelType.Contains(typeof(IThreadChannel)) => ((IThreadChannel)channel).Type switch
			{
				ThreadType.PublicThread => ChannelType.PublicThread,
				ThreadType.PrivateThread => ChannelType.PrivateThread,
				ThreadType.NewsThread => ChannelType.NewsThread,
				_ => throw new NotImplementedException()
			},
			{ } channelType when channelType.Contains(typeof(ITextChannel)) => ChannelType.Text,
			{ } channelType when channelType.Contains(typeof(IDMChannel)) => ChannelType.DM,
			{ } channelType when channelType.Contains(typeof(IVoiceChannel)) => ChannelType.Voice,
			{ } channelType when channelType.Contains(typeof(IGroupChannel)) => ChannelType.Group,
			{ } channelType when channelType.Contains(typeof(ICategoryChannel)) => ChannelType.Category,
			{ } channelType when channelType.Contains(typeof(INewsChannel)) => ChannelType.News,
			{ } channelType when channelType.Contains(typeof(IStageChannel)) => ChannelType.Stage,
			_ => ChannelType.Category
		};
	}
}