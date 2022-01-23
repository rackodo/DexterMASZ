import { AutoModEvent } from "./AutoModEvent";
import { DiscordUser } from "./DiscordUser";

export interface AutoModEventTableEntry {
    autoModerationEvent: AutoModEvent;
    suspect?: DiscordUser;
}