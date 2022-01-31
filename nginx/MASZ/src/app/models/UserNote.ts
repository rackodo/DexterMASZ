export interface UserNote {
    id: number;
    guildId: bigint;
    userId: bigint;
    description: string;
    creatorId: bigint;
    updatedAt: Date;
}