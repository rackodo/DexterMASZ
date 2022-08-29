export interface GuildAuditLogConfig {
    id: number;
    guildId: string;
    guildAuditEvent: number;
    channelId: string;
    pingRoles: string[];
    ignoreRoles: string[];
    ignoreChannels: string[];
}