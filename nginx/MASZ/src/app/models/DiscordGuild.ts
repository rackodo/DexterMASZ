import { DiscordRole } from "./DiscordRole";

export interface DiscordGuild {
  id: bigint;
  name: string;
  iconUrl: string;
  roles: DiscordRole[];
}
