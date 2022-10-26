using Bot.Abstractions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Utilities.Translators;

namespace Utilities.Commands;

public class Banner : Command<Banner>
{
	public DiscordRest Client { get; set; }

	[SlashCommand("banner", "Get the high resolution banner of a user.")]
	public async Task BannerCommand([Summary("user", "User to get the banner from")] IUser user)
	{
		var rUser = await Client.GetRestClient().GetUserAsync(user.Id);

		var embed = new EmbedBuilder()
			.WithTitle(Translator.Get<UtilityTranslator>().BannerUrl())
			.WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {user.Id}")
			.WithColor(Color.Magenta)
			.WithCurrentTimestamp()
			.WithUrl(rUser.GetBannerUrl(size: 1024))
			.WithImageUrl(rUser.GetBannerUrl(size: 1024))
			.WithAuthor(user);

		await Context.Interaction.RespondAsync(embed: embed.Build());
	}
}