export interface ModCaseTemplate {
    id: number;
    userId: bigint;
    templateName: string;
    createdForGuildId: bigint;
    viewPermission: number;
    createdAt: Date;
    caseTitle: string;
    caseDescription: string;
    caseLabels: string[];
    casePunishment: string;
    casePunishmentType: number;
    casePunishedUntil?: Date;
    sendPublicNotification: boolean;
    handlePunishment: boolean;
    announceDm: boolean;
}