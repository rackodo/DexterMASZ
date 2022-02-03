using Discord;
using MASZ.Bot.Models;

namespace MASZ.AutoMods.Models;

public class AutoModEventExpanded
{
	public AutoModEventExpanded(AutoModEvent punishmentsEvent, IUser suspect)
	{
		AutoModEvent = punishmentsEvent;
		Suspect = new DiscordUser(suspect);
	}

	public AutoModEvent AutoModEvent { get; set; }
	public DiscordUser Suspect { get; set; }
}