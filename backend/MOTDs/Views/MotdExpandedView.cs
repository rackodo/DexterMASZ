using Bot.Models;
using Discord;
using MOTDs.Models;

namespace MOTDs.Views;

public class MotdExpandedView(GuildMotd punishmentsEvent, IUser creator)
{
    public MotdView Motd { get; set; } = new MotdView(punishmentsEvent);
    public DiscordUser Creator { get; set; } = DiscordUser.GetDiscordUser(creator);
}
