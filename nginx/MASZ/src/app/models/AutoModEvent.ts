import { AutoModType } from "./AutoModType";

export interface AutoModEvent {
    id: number;
    guildId: string;
    autoModerationType: AutoModType;
    autoModerationAction: number;
    userId: string;
    username: string;
    nickname?: string;
    discriminator: string;
    messageId: string;
    messageContent: string;
    createdAt: Date;
    associatedCaseId?: number;
}