import { DiscordUser } from "./DiscordUser";

export interface GuildUser {
    user: DiscordUser;
    nick?: string;
    roles?: bigint[];
    joined_at?: Date;
}