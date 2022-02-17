export interface GuildAuditConfig {
    id: number;
    guildId: string;
    guildAuditEvent: number;
    channelId: string;
    pingRoles: string[];
}