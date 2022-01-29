using Discord;
using Discord.Interactions;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Attributes;
using MASZ.Bot.Enums;
using MASZ.Bot.Extensions;
using MASZ.Bot.Services;
using MASZ.Bot.Translators;
using MASZ.Utilities.Dynamics;

namespace MASZ.Utilities.Commands;

public class WhoIs : Command<WhoIs>
{
	public ServiceCacher ServiceCacher { get; set; }
	public IServiceProvider ServiceProvider { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("whois", "Who is information about a user.")]
	public async Task WhoIsCommand([Summary("user", "user to scan")] IGuildUser user)
	{
		await Context.Interaction.RespondAsync("Getting WHO IS information...");

		var embed = new EmbedBuilder()
			.WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {user.Id}")
			.WithTimestamp(DateTime.UtcNow)
			.WithColor(Color.Blue)
			.WithDescription(user.Mention)
			.WithAuthor(user)
			.WithThumbnailUrl(user.GetAvatarOrDefaultUrl(size: 1024))
			.AddField(Translator.Get<BotTranslator>().Registered(), user.CreatedAt.DateTime.ToDiscordTs(), true);

		foreach (var repo in ServiceCacher.GetInitializedAuthenticatedClasses<WhoIsResults>(ServiceProvider, Identity))
			await repo.AddWhoIsInformation(embed, user, Context, Translator);

		await Context.Interaction.ModifyOriginalResponseAsync(message =>
		{
			message.Content = "";
			message.Embed = embed.Build();
		});
	}
}