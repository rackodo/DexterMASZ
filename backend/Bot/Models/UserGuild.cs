using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class UserGuild
{
	public UserGuild(IGuildUser user)
	{
		Id = user.Guild.Id;
		Name = user.Guild.Name;
		IconUrl = user.Guild.IconUrl.GetAnimatedOrDefaultAvatar();
		IsAdmin = user.GuildPermissions.Administrator;
	}

	public UserGuild(IUserGuild guild)
	{
		Id = guild.Id;
		Name = guild.Name;
		IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
		IsAdmin = guild.Permissions.Administrator;
	}

	public ulong Id { get; set; }
	public string Name { get; set; }
	public string IconUrl { get; set; }
	public bool IsAdmin { get; set; }
}