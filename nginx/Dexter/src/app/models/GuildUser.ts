import { DiscordUser } from "./DiscordUser";

export interface GuildUser {
    user: DiscordUser;
    nick?: string;
    roles?: string[];
    joined_at?: Date;
}