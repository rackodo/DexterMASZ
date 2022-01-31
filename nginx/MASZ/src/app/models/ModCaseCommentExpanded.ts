import { ModCaseComment } from "./ModCaseComment";
import { DiscordUser } from "./DiscordUser";

export interface ModCaseCommentExpanded {
    comment: ModCaseComment;
    commentor?: DiscordUser;
}