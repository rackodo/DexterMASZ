import { ExperienceRecord } from "../classes/ExperienceRecord";
import { DiscordUser } from "./DiscordUser";

export interface CalcGuildUserLevel {
    userId: bigint;
    guildId: bigint;
    textXp: ExperienceRecord;
    voiceXp: ExperienceRecord;
    totalXp: ExperienceRecord;
    user: DiscordUser;
}
