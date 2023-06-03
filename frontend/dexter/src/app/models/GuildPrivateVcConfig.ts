export interface GuildPrivateVcConfig {
    id: string;
    waitingVcName: string;
    privateCategoryId: string;
    allowedRoles: string[];
    creatorRoles: string[];
    channelFilterRegex: string;
}
