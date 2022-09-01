import { DiscordUser } from "./DiscordUser";
import { ModCaseComment } from "./ModCaseComment";

export interface ModCaseCommentExpanded {
    comment: ModCaseComment;
    commenter?: DiscordUser;
}
