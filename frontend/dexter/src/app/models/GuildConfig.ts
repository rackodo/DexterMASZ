export interface GuildConfig {
    id: number;
    guildId: string;
    modRoles: string[];
    adminRoles: string[];
    staffChannels: string[];
    modNotificationDM: boolean;
    strictModPermissionCheck: boolean;
    executeWhoIsOnJoin: boolean;
    staffWebhook: string;
    adminWebhook: string;
    publishModeratorInfo: boolean;
    preferredLanguage: number;
}
