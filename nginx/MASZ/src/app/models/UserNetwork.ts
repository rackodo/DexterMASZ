import { AutoModEvent } from "./AutoModEvent";
import { DiscordUser } from "./DiscordUser";
import { Guild } from "./Guild";
import { ModCase } from "./ModCase";
import { UserInviteView } from "./UserInviteView";
import { UserMapView } from "./UserMapView";
import { UserNote } from "./UserNote";

export interface UserNetwork {
    guilds: Guild[];
    user : DiscordUser;
    invited: UserInviteView[];
    invitedBy: UserInviteView[];
    modCases: ModCase[];
    modEvents: AutoModEvent[];
    userMaps: UserMapView[];
    userNotes: UserNote[];
}