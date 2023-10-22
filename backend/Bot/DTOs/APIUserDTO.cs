using Bot.Models;
using Discord;

namespace Bot.DTOs;

public class ApiUserDto(List<DiscordGuild> userGuilds, List<DiscordGuild> bannedGuilds,
    List<DiscordGuild> modGuilds, List<DiscordGuild> adminGuilds, IUser user, bool isAdmin = false)
{
    public List<DiscordGuild> UserGuilds { get; set; } = userGuilds;
    public List<DiscordGuild> BannedGuilds { get; set; } = bannedGuilds;
    public List<DiscordGuild> ModGuilds { get; set; } = modGuilds;
    public List<DiscordGuild> AdminGuilds { get; set; } = adminGuilds;
    public DiscordUser DiscordUser { get; set; } = DiscordUser.GetDiscordUser(user);
    public bool IsAdmin { get; set; } = isAdmin;
}
