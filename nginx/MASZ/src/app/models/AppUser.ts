import { DiscordUser } from "./DiscordUser";
import { Guild } from "./Guild";

export interface AppUser {
  userGuilds: Guild[];
  bannedGuilds: Guild[];
  modGuilds: Guild[];
  adminGuilds: Guild[];
  discordUser: DiscordUser;
  isAdmin: boolean;
}