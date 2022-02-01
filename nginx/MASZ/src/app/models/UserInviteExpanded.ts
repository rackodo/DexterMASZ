import { DiscordUser } from "./DiscordUser";
import { UserInvite } from "./UserInvite";

export interface UserInviteExpanded {
    userInvite: UserInvite;
    invitedUser?: DiscordUser;
    invitedBy?: DiscordUser;
}