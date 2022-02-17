import { DiscordUser } from "./DiscordUser";
import { DiscordGuild } from "./DiscordGuild";

export interface AppUser {
  userGuilds: DiscordGuild[];
  bannedGuilds: DiscordGuild[];
  modGuilds: DiscordGuild[];
  adminGuilds: DiscordGuild[];
  discordUser: DiscordUser;
  isAdmin: boolean;
}