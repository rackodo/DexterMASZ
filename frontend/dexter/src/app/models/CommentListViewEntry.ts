import { DiscordUser } from "./DiscordUser";
import { ModCaseComment } from "./ModCaseComment";

export interface CommentListViewEntry {
    comment: ModCaseComment;
    commenter?: DiscordUser;
}
