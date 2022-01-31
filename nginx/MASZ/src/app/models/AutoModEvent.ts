import { AutoModType } from "./AutoModType";

export interface AutoModEvent {
    id: number;
    guildId: bigint;
    autoModerationType: AutoModType;
    autoModerationAction: number;
    userId: bigint;
    username: string;
    nickname?: string;
    discriminator: string;
    messageId: bigint;
    messageContent: string;
    createdAt: Date;
    associatedCaseId?: number;
}