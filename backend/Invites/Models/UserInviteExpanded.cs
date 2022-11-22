using Bot.Models;
using Discord;

namespace Invites.Models;

public class UserInviteExpanded
{
    public UserInvite UserInvite { get; set; }
    public DiscordUser InvitedUser { get; set; }
    public DiscordUser InvitedBy { get; set; }

    public UserInviteExpanded(UserInvite userInvite, IUser invitedUser, IUser invitedBy)
    {
        UserInvite = userInvite;
        InvitedUser = DiscordUser.GetDiscordUser(invitedUser);
        InvitedBy = DiscordUser.GetDiscordUser(invitedBy);
    }
}
