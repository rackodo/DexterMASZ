using Discord;
using MASZ.Bot.Views;
using MASZ.Invites.Models;

namespace MASZ.Invites.Views;

public class UserInviteExpandedView
{
	public UserInviteExpandedView(UserInvite userInvite, IUser invitedUser, IUser invitedBy)
	{
		UserInvite = new UserInviteView(userInvite);
		InvitedUser = new DiscordUserView(invitedUser);
		InvitedBy = new DiscordUserView(invitedBy);
	}

	public UserInviteView UserInvite { get; set; }
	public DiscordUserView InvitedUser { get; set; }
	public DiscordUserView InvitedBy { get; set; }
}