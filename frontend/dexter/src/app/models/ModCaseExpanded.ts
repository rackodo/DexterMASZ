import { DiscordUser } from "./DiscordUser";
import { ModCase } from "./ModCase";
import { ModCaseCommentExpanded } from "./ModCaseCommentExpanded";
import { UserNoteExpanded } from "./UserNoteExpanded";

export interface ModCaseExpanded {
    modCase: ModCase;
    moderator?: DiscordUser;
    lastModerator?: DiscordUser;
    suspect?: DiscordUser;
    lockedBy?: DiscordUser;
    deletedBy?: DiscordUser;
    comments: ModCaseCommentExpanded[];
    punishmentProgress?: number;
    userNote?: UserNoteExpanded;
}
