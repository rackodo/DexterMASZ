export interface ModCaseTemplate {
    id: number;
    userId: string;
    templateName: string;
    createdForGuildId: string;
    viewPermission: number;
    createdAt: Date;
    caseTitle: string;
    caseDescription: string;
    caseLabels: string[];
    casePunishment: string;
    caseSeverityType: number;
    casePunishmentType: number;
    casePunishedUntil?: Date;
    handlePunishment: boolean;
}