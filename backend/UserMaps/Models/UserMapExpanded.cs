using Bot.Models;
using Discord;

namespace UserMaps.Models;

public class UserMapExpanded(UserMap userMaps, IUser userA, IUser userB, IUser moderator)
{
    public UserMap UserMap { get; set; } = userMaps;
    public DiscordUser UserA { get; set; } = DiscordUser.GetDiscordUser(userA);
    public DiscordUser UserB { get; set; } = DiscordUser.GetDiscordUser(userB);
    public DiscordUser Moderator { get; set; } = DiscordUser.GetDiscordUser(moderator);
}
