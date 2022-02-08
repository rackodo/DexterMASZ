using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordGuild
{
	public DiscordGuild(IGuild guild)
	{
		Id = guild.Id;
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		Roles = new List<DiscordRole>();

		foreach (var role in guild.Roles)
			Roles.Add(new DiscordRole(role));

		Channels = new List<DiscordChannel>();

		foreach (var channel in guild.GetTextChannelsAsync().GetAwaiter().GetResult())
			Channels.Add(new DiscordChannel(channel));
	}

	public DiscordGuild(UserGuild guild)
	{
		Id = guild.Id;
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		Roles = new List<DiscordRole>();
		Channels = new List<DiscordChannel>();
	}

	public ulong Id { get; set; }
	public string Name { get; set; }
	public string IconUrl { get; set; }
	public List<DiscordRole> Roles { get; set; }
	public List<DiscordChannel> Channels { get; set; }
}