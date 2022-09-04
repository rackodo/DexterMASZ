import { DiscordGuild } from "./DiscordGuild";

export interface GuildDeleteDialogData {
    guild: DiscordGuild;
    deleteData: boolean;
}
