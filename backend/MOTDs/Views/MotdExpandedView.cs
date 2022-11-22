using Bot.Models;
using Discord;
using MOTDs.Models;

namespace MOTDs.Views;

public class MotdExpandedView
{
    public MotdView Motd { get; set; }
    public DiscordUser Creator { get; set; }

    public MotdExpandedView(GuildMotd punishmentsEvent, IUser creator)
    {
        Motd = new MotdView(punishmentsEvent);
        Creator = DiscordUser.GetDiscordUser(creator);
    }
}
