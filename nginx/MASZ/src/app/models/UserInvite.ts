export interface UserInvite {
    id: number;
    guildId: bigint;
    targetChannelId?: bigint;
    joinedUserId: bigint;
    usedInvite: string;
    inviteIssuerId: bigint;
    joinedAt: Date;
    inviteCreatedAt: Date;
}