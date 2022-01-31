import { DiscordUser } from "./DiscordUser";
import { ModCase } from "./ModCase";

export interface ModCaseComment {
    id: number;
    message: string;
    modCase?: ModCase;
    userId: bigint;
    createdAt: Date;
}