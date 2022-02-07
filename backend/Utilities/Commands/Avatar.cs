using Bot.Abstractions;
using Bot.Extensions;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Utilities.Translators;

namespace Utilities.Commands;

public class Avatar : Command<Avatar>
{
	[SlashCommand("avatar", "Get the high resolution avatar of a user.")]
	public async Task AvatarCommand([Summary("user", "User to get the avatar from")] IUser user)
	{
		var embed = new EmbedBuilder()
			.WithTitle(Translator.Get<UtilityTranslator>().AvatarUrl())
			.WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {user.Id}")
			.WithColor(Color.Magenta)
			.WithCurrentTimestamp()
			.WithUrl(user.GetAvatarOrDefaultUrl(size: 1024))
			.WithImageUrl(user.GetAvatarOrDefaultUrl(size: 1024))
			.WithAuthor(user);

		try
		{
			var gUser = Context.Guild.GetUser(user.Id);

			if (gUser is { GuildAvatarId: { } })
				embed.WithUrl(gUser.GetGuildAvatarUrl(size: 1024))
					.WithImageUrl(gUser.GetGuildAvatarUrl(size: 1024))
					.WithAuthor(gUser);
		}
		catch
		{
			// ignored
		}

		await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
	}
}