import { AutoModAction } from "./AutoModAction";
import { AutoModType } from "./AutoModType";
import { ChannelNotificationBehavior } from "./ChannelNotificationBehavior";
import { PunishmentType } from "./PunishmentType";

export interface AutoModConfig {
    id: number;
    guildId: string;
    autoModType: AutoModType;
    autoModAction: AutoModAction;
    punishmentType: PunishmentType;
    punishmentDurationMinutes: number;
    ignoreChannels: string[];
    ignoreRoles: string[];
    timeLimitMinutes?: number;
    limit?: number;
    customWordFilter?: string;
    sendDmNotification: boolean;
    sendPublicNotification: boolean;
    channelNotificationBehavior: ChannelNotificationBehavior;
}

