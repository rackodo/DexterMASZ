using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Utilities.Dynamics;
using Utilities.Translators;

namespace Utilities.Commands;

public class WhoIs : Command<WhoIs>
{
	public CachedServices CachedServices { get; set; }
	public IServiceProvider ServiceProvider { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("whois", "Who is information about a user.")]
	public async Task WhoIsCommand([Summary("user", "user to scan")] IGuildUser user)
	{
		GuildConfigRepository.AsUser(user);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var embed = new EmbedBuilder()
			.WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {user.Id}")
			.WithTitle($"{Translator.Get<UtilityTranslator>().UserProfile()} {user.Username}#{user.Discriminator}")
			.WithCurrentTimestamp()
			.WithColor(Color.Blue)
			.WithThumbnailUrl(user.GetAvatarOrDefaultUrl(size: 1024))
			.AddField(Translator.Get<BotTranslator>().Registered(), user.CreatedAt.DateTime.ToDiscordTs(), true);

		foreach (var repo in CachedServices.GetInitializedAuthenticatedClasses<WhoIsResults>(ServiceProvider, Identity))
			await repo.AddWhoIsInformation(embed, user, Context, Translator);

		await Context.Interaction.ModifyOriginalResponseAsync(message =>
		{
			message.Embed = embed.Build();
		});
	}
}