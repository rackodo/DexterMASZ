import { PunishmentType } from "./PunishmentType";

export interface ModCaseFilter {
    customTextFilter?: string;
    userIds?: bigint[];
    moderatorIds?: bigint[];
    since?: string;
    before?: string;
    punishedUntilMin?: string;
    punishedUntilMax?: string;
    edited?: boolean;
    creationTypes?: number[];
    punishmentTypes?: PunishmentType[];
    punishmentActive?: boolean;
    lockedComments?: boolean;
    markedToDelete?: boolean;
}
