import { ApiEnum } from "./ApiEnum";
import { ModCaseComment } from "./ModCaseComment";
import { PunishmentType } from "./PunishmentType";

export interface ModCase {
    id: number;
    caseId: number;
    guildId: bigint;
    title: string;
    description: string;
    userId: bigint;
    username: string;
    discriminator?: string;
    nickname?: string;
    modId?: bigint;
    createdAt: Date;
    occuredAt: Date;
    lastEditedAt: Date;
    lastEditedByModId?: bigint;
    labels: string[];
    others?: string;
    valid: boolean;
    creationType: number;
    punishmentType: PunishmentType;
    punishedUntil: Date;
    punishmentActive: boolean;
    allowComments: boolean;
    lockedAt: Date;
    lockedByUserId: bigint;
    markedToDeleteAt?: Date;
    deletedByUserId?: bigint;
    comments: ModCaseComment[];
}

export function convertModCaseToPunishmentString(modcase?: ModCase, punishments?: ApiEnum[]): string {
    return punishments?.find(x => x.key === modcase?.punishmentType)?.value ?? "Unknown";
}
