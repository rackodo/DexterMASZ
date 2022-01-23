import { ApiEnum } from "./ApiEnum";
import { CaseComment } from "./CaseComment";
import { PunishmentType } from "./PunishmentType";

export interface ModCase {
    id: number;
    caseId: number;
    guildId: string;
    title: string;
    description: string;
    userId: string;
    username: string;
    discriminator?: string;
    nickname?: string;
    modId?: string;
    createdAt: Date;
    occuredAt: Date;
    lastEditedAt: Date;
    lastEditedByModId?: string;
    labels: string[];
    others?: string;
    valid: boolean;
    creationType: number;
    punishmentType: PunishmentType;
    punishedUntil: Date;
    punishmentActive: boolean;
    allowComments: boolean;
    lockedAt: Date;
    lockedByUserId: string;
    markedToDeleteAt?: Date;
    deletedByUserId?: string;
    comments: CaseComment[];
}

export function convertModCaseToPunishmentString(modcase?: ModCase, punishments?: ApiEnum[]): string {
    return punishments?.find(x => x.key === modcase?.punishmentType)?.value ?? "Unknown";
}
