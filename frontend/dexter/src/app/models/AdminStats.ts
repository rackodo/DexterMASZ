import { ServiceStatus } from "./ServiceStatus";

export interface AdminStats {
    botStatus: ServiceStatus;
    dbStatus: ServiceStatus;
    cacheStatus: ServiceStatus;
    loginsInLast15Minutes: string[];
    defaultLanguage: number;
    trackedInvites: number;
    modCases: number;
    guilds: number;
    autoModEvents: number;
    userNotes: number;
    userMaps: number;
    apiTokens: number;
    nextCache: Date;
    cachedDataFromDiscord: string[];
}
