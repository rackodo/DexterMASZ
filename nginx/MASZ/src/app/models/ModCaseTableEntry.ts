import { DiscordUser } from "./DiscordUser";
import { ModCase } from "./ModCase";

export interface ModCaseTableEntry {
    modCase: ModCase;
    moderator?: DiscordUser;
    suspect?: DiscordUser;
}