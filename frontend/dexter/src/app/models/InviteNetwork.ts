import { DiscordGuild } from "./DiscordGuild";
import { UserInviteExpanded } from "./UserInviteExpanded";

export interface InviteNetwork {
    guild?: DiscordGuild;
    invites: UserInviteExpanded[];
}
