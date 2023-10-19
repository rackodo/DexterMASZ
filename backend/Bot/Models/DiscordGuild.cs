using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordGuild
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string IconUrl { get; set; }
    public List<DiscordRole> Roles { get; set; }
    public List<DiscordChannel> Channels { get; set; }

    private DiscordGuild(IGuild guild)
    {
        Id = guild.Id;
        Name = guild.Name;
        IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
        Roles = [];

        foreach (var role in guild.Roles)
            Roles.Add(new DiscordRole(role));

        Channels = [];

        foreach (var channel in guild.GetTextChannelsAsync().GetAwaiter().GetResult())
            Channels.Add(DiscordChannel.GetDiscordChannel(channel));
    }

    public DiscordGuild(UserGuild guild)
    {
        Id = guild.Id;
        Name = guild.Name;
        IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
        Roles = [];
        Channels = [];
    }

    public static DiscordGuild GetDiscordGuild(IGuild guild) => guild is null ? null : guild.Id is 0 ? null : new DiscordGuild(guild);
}
