import { PunishmentType } from "./PunishmentType";
import { SeverityType } from "./SeverityType";

export interface ModCaseFilter {
    customTextFilter?: string;
    userIds?: string[];
    moderatorIds?: string[];
    since?: string;
    before?: string;
    punishedUntilMin?: string;
    punishedUntilMax?: string;
    edited?: boolean;
    creationTypes?: number[];
    punishmentTypes?: PunishmentType[];
    severityTypes?: SeverityType[];
    punishmentActive?: boolean;
    lockedComments?: boolean;
    markedToDelete?: boolean;
}
