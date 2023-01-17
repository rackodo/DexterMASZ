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
        if (guildUser is null)
            return null;
        return guildUser.Id is 0 ? null : new UserGuild(guildUser);
    }

    public static UserGuild GetUserGuild(IUserGuild userGuild)
    {
        if (userGuild is null)
            return null;
        return userGuild.Id is 0 ? null : new UserGuild(userGuild);
    }
}
