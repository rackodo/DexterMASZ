using Bot.Models;
using Discord;

namespace AutoMods.Models;

public class AutoModEventExpanded(AutoModEvent punishmentsEvent, IUser suspect)
{
    public AutoModEvent AutoModEvent { get; set; } = punishmentsEvent;
    public DiscordUser Suspect { get; set; } = DiscordUser.GetDiscordUser(suspect);
}
