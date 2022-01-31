export interface GuildAuditConfig {
    id: number;
    guildId: bigint;
    guildAuditLogEvent: number;
    channelId: bigint;
    pingRoles: bigint[];
}