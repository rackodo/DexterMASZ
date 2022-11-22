using Bot.Models;
using Discord;

namespace Bot.DTOs;

public class ApiUserDto
{
    public List<DiscordGuild> UserGuilds { get; set; }
    public List<DiscordGuild> BannedGuilds { get; set; }
    public List<DiscordGuild> ModGuilds { get; set; }
    public List<DiscordGuild> AdminGuilds { get; set; }
    public DiscordUser DiscordUser { get; set; }
    public bool IsAdmin { get; set; }

    public ApiUserDto(List<DiscordGuild> userGuilds, List<DiscordGuild> bannedGuilds,
        List<DiscordGuild> modGuilds, List<DiscordGuild> adminGuilds, IUser user, bool isAdmin = false)
    {
        UserGuilds = userGuilds;
        BannedGuilds = bannedGuilds;
        ModGuilds = modGuilds;
        AdminGuilds = adminGuilds;
        DiscordUser = DiscordUser.GetDiscordUser(user);
        IsAdmin = isAdmin;
    }
}
