import { AutoModEvent } from "./AutoModEvent";
import { DiscordUser } from "./DiscordUser";

export interface AutoModEventTableEntry {
    autoModEvent: AutoModEvent;
    suspect?: DiscordUser;
}