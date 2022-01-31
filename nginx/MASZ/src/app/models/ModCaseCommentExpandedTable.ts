import { ModCaseCommentExpanded } from "./ModCaseCommentExpanded";

export interface ModCaseCommentExpandedTable extends ModCaseCommentExpanded {
    guildId: string;
    caseId: number;
}