export interface DiscordUser {
  id: bigint;
  username: string;
  discriminator: string;
  avatar: string;
  bot: boolean;
  imageUrl: string;
}