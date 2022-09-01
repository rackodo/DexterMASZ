export interface GuildAuditLogRuleDefinition {
    type: number;
    key: string;
    comingSoon?: boolean;
    channelFilter?: boolean;
    roleFilter?: boolean;
}
