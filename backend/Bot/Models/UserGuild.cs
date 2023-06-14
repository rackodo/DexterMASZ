using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class UserGuild
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string IconUrl { get; set; }
    public bool IsAdmin { get; set; }

    private UserGuild(IGuildUser user)
    {
        Id = user.Guild.Id;
        Name = user.Guild.Name;
        IconUrl = user.Guild.IconUrl.GetAnimatedOrDefaultAvatar();
        IsAdmin = user.GuildPermissions.Administrator;
    }

    private UserGuild(IUserGuild guild)
    {
        Id = guild.Id;
        Name = guild.Name;
        IconUrl = guild.IconUrl.GetAnimatedOrDefaultAvatar();
        IsAdmin = guild.Permissions.Administrator;
    }

    public static UserGuild GetUserGuild(IGuildUser guildUser)
    {
        return guildUser is null ? null : guildUser.Id is 0 ? null : new UserGuild(guildUser);
    }

    public static UserGuild GetUserGuild(IUserGuild userGuild)
    {
        return userGuild is null ? null : userGuild.Id is 0 ? null : new UserGuild(userGuild);
    }
}
