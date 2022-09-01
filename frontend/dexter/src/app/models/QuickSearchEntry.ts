import { AutoModEventTableEntry } from "./AutoModEventTableEntry";
import { ModCaseTableEntry } from "./ModCaseTableEntry";

export interface QuickSearchEntry {
    entry: ModCaseTableEntry|AutoModEventTableEntry;
    createdAt: Date;
    quickSearchEntryType: number;
}
