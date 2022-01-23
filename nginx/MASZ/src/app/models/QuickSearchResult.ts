import { QuickSearchEntry } from "./QuickSearchEntry";
import { UserMapView } from "./UserMapView";
import { UserNoteView } from "./UserNoteView";

export interface QuickSearchResult {
    searchEntries: QuickSearchEntry[];
    userNoteView: UserNoteView;
    userMapViews: UserMapView[];
}