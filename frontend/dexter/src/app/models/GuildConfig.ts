export interface GuildConfig {
    id: number;
    guildId: string;
    modRoles: string[];
    adminRoles: string[];
    staffChannels: string[];
    modNotificationDM: boolean;
    strictModPermissionCheck: boolean;
    executeWhoIsOnJoin: boolean;
    modNotificationWebhook: string;
    publishModeratorInfo: boolean;
    preferredLanguage: number;
}
