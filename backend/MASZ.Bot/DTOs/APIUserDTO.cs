using Discord;
using MASZ.Bot.Views;

namespace MASZ.Bot.DTOs;

public class ApiUserDto
{
	public ApiUserDto(List<DiscordGuildView> userGuilds, List<DiscordGuildView> bannedGuilds,
		List<DiscordGuildView> modGuilds, List<DiscordGuildView> adminGuilds, IUser user, bool isAdmin = false)
	{
		UserGuilds = userGuilds;
		BannedGuilds = bannedGuilds;
		ModGuilds = modGuilds;
		AdminGuilds = adminGuilds;
		DiscordUser = new DiscordUserView(user);
		IsAdmin = isAdmin;
	}

	public List<DiscordGuildView> UserGuilds { get; set; }
	public List<DiscordGuildView> BannedGuilds { get; set; }
	public List<DiscordGuildView> ModGuilds { get; set; }
	public List<DiscordGuildView> AdminGuilds { get; set; }
	public DiscordUserView DiscordUser { get; set; }
	public bool IsAdmin { get; set; }
}