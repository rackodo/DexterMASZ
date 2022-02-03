export interface GuildConfig {
    id: number;
    guildId: string;
    modRoles: string[];
    adminRoles: string[];
    mutedRoles: string[];
    modNotificationDM: boolean;
    strictModPermissionCheck: boolean;
    executeWhoIsOnJoin: boolean;
    modPublicNotificationWebhook: string;
    modInternalNotificationWebhook: string;
    publishModeratorInfo: boolean;
    preferredLanguage: number;
}