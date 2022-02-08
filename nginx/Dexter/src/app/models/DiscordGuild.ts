import { DiscordRole } from "./DiscordRole";
import { DiscordChannel } from "./DiscordChannel";

export interface DiscordGuild {
  id: string;
  name: string;
  iconUrl: string
  roles: DiscordRole[];
  channels: DiscordChannel[];
}
