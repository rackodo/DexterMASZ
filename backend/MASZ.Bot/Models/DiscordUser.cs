using Discord;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Extensions;

namespace MASZ.Bot.Models;

public class DiscordUser
{
	public DiscordUser(IUser user)
	{
		if (user is null)
			throw new ResourceNotFoundException("User for DiscordUserView is equal to null!");

		if (user.Id is 0)
			throw new ResourceNotFoundException("User for DiscordUserView has an ID of 0!");

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