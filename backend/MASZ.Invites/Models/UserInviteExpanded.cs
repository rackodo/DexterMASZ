using Discord;
using MASZ.Bot.Models;

namespace MASZ.Invites.Models;

public class UserInviteExpanded
{
	public UserInviteExpanded(UserInvite userInvite, IUser invitedUser, IUser invitedBy)
	{
		UserInvite = userInvite;
		InvitedUser = new DiscordUser(invitedUser);
		InvitedBy = new DiscordUser(invitedBy);
	}

	public UserInvite UserInvite { get; set; }
	public DiscordUser InvitedUser { get; set; }
	public DiscordUser InvitedBy { get; set; }
}