export interface GuildAuditRuleDefinition {
    type: number;
    key: string;
    comingSoon?: boolean;
    channelFilter?: boolean;
    roleFilter?: boolean;
}