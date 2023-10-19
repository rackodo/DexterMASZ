using Bot.Models;
using Discord;

namespace Invites.Models;

public class UserInviteExpanded(UserInvite userInvite, IUser invitedUser, IUser invitedBy)
{
    public UserInvite UserInvite { get; set; } = userInvite;
    public DiscordUser InvitedUser { get; set; } = DiscordUser.GetDiscordUser(invitedUser);
    public DiscordUser InvitedBy { get; set; } = DiscordUser.GetDiscordUser(invitedBy);
}
