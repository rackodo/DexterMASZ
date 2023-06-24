using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordUser : DiscordUserPartial
{
    public string ImageUrl { get; set; }
    public string Locale { get; set; }
    public string Avatar { get; set; }
    public bool Bot { get; set; }

    private DiscordUser(IUser user) : base(user)
    {
        ImageUrl = user.GetAvatarOrDefaultUrl(size: 512);
        Locale = user is ISelfUser sUser ? sUser.Locale : "en-US";
        Avatar = user.AvatarId;
        Bot = user.IsBot;
    }

    public static DiscordUser GetDiscordUser(IUser user) =>
        user is null ? null : user.Id is 0 ? null : new DiscordUser(user);
}
