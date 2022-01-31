export interface ScheduledMessage {
    id: number;
    name: string;
    content: string;
    scheduledFor: Date;
    status: ScheduledMessageExtendedStatus;
    guildId: bigint;
    channelId: bigint;
    creatorId: bigint;
    lastEditedById: bigint;
    createdAt: Date;
    lastEditedAt: Date;
    failureReason?: FailureReason;
}

export enum ScheduledMessageExtendedStatus {
    Pending = 0,
    Sent = 1,
    Failed = 2
}

export enum FailureReason {
    Unknown = 0,
    ChannelNotFound = 1,
    InsufficientPermissions = 2,
}