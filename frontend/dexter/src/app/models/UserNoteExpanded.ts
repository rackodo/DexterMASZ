import { DiscordUser } from "./DiscordUser";
import { UserNote } from "./UserNote";

export interface UserNoteExpanded {
    userNote: UserNote;
    user?: DiscordUser;
    moderator?: DiscordUser;
}
