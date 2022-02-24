using Discord;
using Discord.WebSocket;

namespace Bot.Extensions;

public static class SendMessage
{
	public static async Task SendEmbed(this DiscordSocketClient client,
		ulong guildId, ulong channelId, EmbedBuilder embed)
	{
		await client.GetGuild(guildId).GetTextChannel(channelId)
			.SendMessageAsync(embed: embed.Build(), allowedMentions: AllowedMentions.None);
	}
}
