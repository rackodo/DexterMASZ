import { DiscordUser } from "./DiscordUser";
import { DiscordChannel } from "./DiscordChannel";
import { ScheduledMessage } from "./ScheduledMessage";

export interface ScheduledMessageExtended {
	scheduledMessage: ScheduledMessage;
    creator?: DiscordUser;
    lastEdited?: DiscordUser;
    channel?: DiscordChannel;
}

