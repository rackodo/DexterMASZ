export interface UserMap {
    id: number;
    guildId: bigint;
    userA: bigint;
    userB: bigint;
    creatorUserId: bigint;
    createdAt: Date;
    reason: string;
}