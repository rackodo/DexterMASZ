using Bot.Models;
using Discord;

namespace UserMaps.Models;

public class UserMapExpanded
{
    public UserMap UserMap { get; set; }
    public DiscordUser UserA { get; set; }
    public DiscordUser UserB { get; set; }
    public DiscordUser Moderator { get; set; }

    public UserMapExpanded(UserMap userMaps, IUser userA, IUser userB, IUser moderator)
    {
        UserMap = userMaps;
        UserA = DiscordUser.GetDiscordUser(userA);
        UserB = DiscordUser.GetDiscordUser(userB);
        Moderator = DiscordUser.GetDiscordUser(moderator);
    }
}