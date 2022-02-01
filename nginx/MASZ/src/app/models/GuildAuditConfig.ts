export interface GuildAuditConfig {
    id: number;
    guildId: string;
    guildAuditLogEvent: number;
    channelId: string;
    pingRoles: string[];
}