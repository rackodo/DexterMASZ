import { QuickSearchEntry } from "./QuickSearchEntry";
import { UserMapExpanded } from "./UserMapExpanded";
import { UserNoteExpanded } from "./UserNoteExpanded";

export interface QuickSearchResult {
    searchEntries: QuickSearchEntry[];
    userNoteExpanded: UserNoteExpanded;
    userMapExpands: UserMapExpanded[];
}