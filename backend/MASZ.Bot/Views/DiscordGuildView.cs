using Discord;
using MASZ.Bot.Extensions;
using MASZ.Bot.Models;

namespace MASZ.Bot.Views;

public class DiscordGuildView
{
	public DiscordGuildView(IGuild guild)
	{
		Id = guild.Id.ToString();
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		Roles = new List<DiscordRoleView>();

		foreach (var role in guild.Roles)
			Roles.Add(new DiscordRoleView(role));
	}

	public DiscordGuildView(UserGuild guild)
	{
		Id = guild.Id.ToString();
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		Roles = new List<DiscordRoleView>();
	}

	public string Id { get; set; }
	public string Name { get; set; }
	public string IconUrl { get; set; }
	public List<DiscordRoleView> Roles { get; set; }
}