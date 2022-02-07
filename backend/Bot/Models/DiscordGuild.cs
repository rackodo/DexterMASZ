using Discord;
using Bot.Extensions;

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
	}

	public DiscordGuild(UserGuild guild)
	{
		Id = guild.Id;
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		Roles = new List<DiscordRole>();
	}

	public ulong Id { get; set; }
	public string Name { get; set; }
	public string IconUrl { get; set; }
	public List<DiscordRole> Roles { get; set; }
}