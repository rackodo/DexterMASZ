import { AutoModEventTableEntry } from "./AutoModEventTableEntry";
import { IModCaseTableEntry } from "./IModCaseTableEntry";

export interface QuickSearchEntry {
    entry: IModCaseTableEntry|AutoModEventTableEntry;
    createdAt: Date;
    quickSearchEntryType: number;
}