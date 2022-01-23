import { DiscordUser } from "./DiscordUser";
import { UserMap } from "./UserMap";

export interface UserMapView {
    userMap: UserMap;
    userA?: DiscordUser;
    userB?: DiscordUser;
    moderator?: DiscordUser;
}