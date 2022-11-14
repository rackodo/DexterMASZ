using Bot.Models;
using Discord;

namespace AutoMods.Models;

public class AutoModEventExpanded
{
    public AutoModEvent AutoModEvent { get; set; }
    public DiscordUser Suspect { get; set; }

    public AutoModEventExpanded(AutoModEvent punishmentsEvent, IUser suspect)
    {
        AutoModEvent = punishmentsEvent;
        Suspect = DiscordUser.GetDiscordUser(suspect);
    }
}