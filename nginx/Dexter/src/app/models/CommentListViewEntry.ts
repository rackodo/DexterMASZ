import { ModCaseComment } from "./ModCaseComment";
import { DiscordUser } from "./DiscordUser";

export interface CommentListViewEntry {
    comment: ModCaseComment;
    commenter?: DiscordUser;
}