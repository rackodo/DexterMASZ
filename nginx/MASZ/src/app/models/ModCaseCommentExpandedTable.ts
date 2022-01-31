import { ModCaseCommentExpanded } from "./ModCaseCommentExpanded";

export interface ModCaseCommentExpandedTable extends ModCaseCommentExpanded {
    guildId: bigint;
    caseId: number;
}