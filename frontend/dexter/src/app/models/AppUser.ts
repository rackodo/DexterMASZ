import { DiscordGuild } from "./DiscordGuild";
import { DiscordUser } from "./DiscordUser";

export interface AppUser {
  userGuilds: DiscordGuild[];
  bannedGuilds: DiscordGuild[];
  modGuilds: DiscordGuild[];
  adminGuilds: DiscordGuild[];
  discordUser: DiscordUser;
  isAdmin: boolean;
}
