using Discord;
using MASZ.AutoMods.Models;
using MASZ.Bot.Views;

namespace MASZ.AutoMods.Views;

public class AutoModEventExpandedView
{
	public AutoModEventExpandedView(AutoModEvent punishmentsEvent, IUser suspect)
	{
		AutoModerationEvent = new AutoModEventView(punishmentsEvent);
		Suspect = new DiscordUserView(suspect);
	}

	public AutoModEventView AutoModerationEvent { get; set; }
	public DiscordUserView Suspect { get; set; }
}