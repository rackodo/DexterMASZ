import { AutoModAction } from "./AutoModAction";
import { AutoModType } from "./AutoModType";
import { ChannelNotificationBehavior } from "./ChannelNotificationBehavior";
import { PunishmentType } from "./PunishmentType";

export interface AutoModConfig {
    id: number;
    guildId: bigint;
    autoModerationType: AutoModType;
    autoModerationAction: AutoModAction;
    punishmentType: PunishmentType;
    punishmentDurationMinutes: number;
    ignoreChannels: bigint[];
    ignoreRoles: bigint[];
    timeLimitMinutes?: number;
    limit?: number;
    customWordFilter?: string;
    sendDmNotification: boolean;
    sendPublicNotification: boolean;
    channelNotificationBehavior: ChannelNotificationBehavior;
}

