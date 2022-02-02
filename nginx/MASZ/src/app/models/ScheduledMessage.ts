import { DiscordUser } from "./DiscordUser";
import { DiscordChannel } from "./DiscordChannel";

export interface ScheduledMessage {
    id: number;
    name: string;
    content: string;
    scheduledFor: Date;
    status: ScheduledMessageStatus;
    guildId: string;
    channelId: string;
    creatorId: string;
    lastEditedById: string;
    createdAt: Date;
    lastEditedAt: Date;
    failureReason?: FailureReason;

    creator?: DiscordUser;
    lastEdited?: DiscordUser;
    channel?: DiscordChannel;
}

export enum ScheduledMessageStatus {
    Pending = 0,
    Sent = 1,
    Failed = 2
}

export enum FailureReason {
    Unknown = 0,
    ChannelNotFound = 1,
    InsufficientPermissions = 2,
}

