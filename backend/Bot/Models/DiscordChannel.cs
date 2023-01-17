using Discord;

namespace Bot.Models;

public class DiscordChannel
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public int Position { get; set; }
    public int Type { get; set; }

    private DiscordChannel(IGuildChannel channel)
    {
        Id = channel.Id;
        Name = channel.Name;
        Position = channel.Position;
        Type = (int)channel.GetChannelType();
    }

    public static DiscordChannel GetDiscordChannel(IGuildChannel channel) =>
        channel is null
            ? null
            : channel.Id is 0 || !channel.GetChannelType().HasValue ?
                null :
                new DiscordChannel(channel);
}
