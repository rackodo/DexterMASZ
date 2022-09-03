using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordUser
{
	public static DiscordUser GetDiscordUser(IUser user)
	{
		if (user is null)
			return null;
		else if (user.Id is 0)
			return null;
		else
			return DiscordUser.GetDiscordUser(user);
	}

	private DiscordUser(IUser user)
	{
		Id = user.Id;
		Username = user.Username;
		Discriminator = user.Discriminator;
		ImageUrl = user.GetAvatarOrDefaultUrl(size: 512);
		Locale = user is ISelfUser sUser ? sUser.Locale : "en-US";
		Avatar = user.AvatarId;
		Bot = user.IsBot;
	}

	public ulong Id { get; set; }
	public string Username { get; set; }
	public string Discriminator { get; set; }
	public string ImageUrl { get; set; }
	public string Locale { get; set; }
	public string Avatar { get; set; }
	public bool Bot { get; set; }
}