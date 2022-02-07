using Discord;
using Bot.Models;

namespace UserMaps.Models;

public class UserMapExpanded
{
	public UserMapExpanded(UserMap userMaps, IUser userA, IUser userB, IUser moderator)
	{
		UserMap = userMaps;
		UserA = new DiscordUser(userA);
		UserB = new DiscordUser(userB);
		Moderator = new DiscordUser(moderator);
	}

	public UserMap UserMap { get; set; }
	public DiscordUser UserA { get; set; }
	public DiscordUser UserB { get; set; }
	public DiscordUser Moderator { get; set; }
}