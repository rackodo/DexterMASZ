export interface GuildConfig {
    id: number;
    guildId: bigint;
    modRoles: bigint[];
    adminRoles: bigint[];
    mutedRoles: bigint[];
    modNotificationDM: boolean;
    strictModPermissionCheck: boolean;
    executeWhoisOnJoin: boolean;
    modPublicNotificationWebhook: string;
    modInternalNotificationWebhook: string;
    publishModeratorInfo: boolean;
    preferredLanguage: number;
}