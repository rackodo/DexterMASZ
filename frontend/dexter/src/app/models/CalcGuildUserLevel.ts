import { ExperienceRecord } from "../classes/ExperienceRecord";

export interface CalcGuildUserLevel {
    userId: bigint;
    guildId: bigint;
    textXp: ExperienceRecord;
    voiceXp: ExperienceRecord;
    totalXp: ExperienceRecord;
}
