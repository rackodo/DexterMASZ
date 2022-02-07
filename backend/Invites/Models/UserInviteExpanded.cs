using Bot.Models;
using Discord;

namespace Invites.Models;

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