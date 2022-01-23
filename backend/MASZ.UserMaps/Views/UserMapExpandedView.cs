using Discord;
using MASZ.Bot.Views;
using MASZ.UserMaps.Models;

namespace MASZ.UserMaps.Views;

public class UserMapExpandedView
{
	public UserMapExpandedView(UserMap userMaps, IUser userA, IUser userB, IUser moderator)
	{
		UserMap = new UserMapView(userMaps);
		UserA = new DiscordUserView(userA);
		UserB = new DiscordUserView(userB);
		Moderator = new DiscordUserView(moderator);
	}

	public UserMapView UserMap { get; set; }
	public DiscordUserView UserA { get; set; }
	public DiscordUserView UserB { get; set; }
	public DiscordUserView Moderator { get; set; }
}