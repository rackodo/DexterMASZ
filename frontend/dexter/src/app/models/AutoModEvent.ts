import { AutoModType } from "./AutoModType";

export interface AutoModEvent {
    id: number;
    guildId: string;
    autoModType: AutoModType;
    autoModAction: number;
    userId: string;
    username: string;
    nickname?: string;
    messageId: string;
    messageContent: string;
    createdAt: Date;
    associatedCaseId?: number;
}
