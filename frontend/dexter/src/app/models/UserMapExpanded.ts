import { DiscordUser } from "./DiscordUser";
import { UserMap } from "./UserMap";

export interface UserMapExpanded {
    userMap: UserMap;
    userA?: DiscordUser;
    userB?: DiscordUser;
    moderator?: DiscordUser;
}
