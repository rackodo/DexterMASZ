using Discord;
using MASZ.Bot.Views;
using MASZ.MOTDs.Models;

namespace MASZ.MOTDs.Views;

public class MotdExpandedView
{
	public MotdExpandedView(GuildMotd punishmentsEvent, IUser creator)
	{
		Motd = new MotdView(punishmentsEvent);
		Creator = new DiscordUserView(creator);
	}

	public MotdView Motd { get; set; }
	public DiscordUserView Creator { get; set; }
}