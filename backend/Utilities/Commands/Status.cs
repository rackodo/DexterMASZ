using Discord;
using Discord.Interactions;
using Humanizer;
using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Extensions;
using Bot.Identities;
using Utilities.Translators;
using System.Text;

namespace Utilities.Commands;

public class Status : Command<Status>
{
	public StatusRepository StatusRepository { get; set; }

	[Require(RequireCheck.SiteAdmin)]
	[SlashCommand("status", "See the current status of your application.")]
	public async Task StatusCommand()
	{
		await Context.Interaction.DeferAsync(ephemeral: true);

		var embed = new EmbedBuilder()
			.WithTitle(Translator.Get<UtilityTranslator>().Status())
			.WithColor(Color.Green)
			.WithCurrentTimestamp();

		var botDetails = StatusRepository.GetBotStatus();
		var dbDetails = await StatusRepository.GetDbStatus();
		var cacheDetails = StatusRepository.GetCacheStatus();

		var lastDisconnect = string.Empty;

		if (botDetails.LastDisconnect != null)
			lastDisconnect = Translator.Get<UtilityTranslator>().LastDisconnectAt(botDetails.LastDisconnect.Value.ToDiscordTs());

		embed.AddField(
				botDetails.Online.GetCheckEmoji() + " " + Translator.Get<UtilityTranslator>().Bot(),
				$"{botDetails.ResponseTime:0.0}ms\n{lastDisconnect}",
				false
			)
			.AddField(
				dbDetails.Online.GetCheckEmoji() + " " + Translator.Get<UtilityTranslator>().Database(),
				$"{dbDetails.ResponseTime:0.0}ms",
				false
			)
			.AddField(
				cacheDetails.Online.GetCheckEmoji() + " " + Translator.Get<UtilityTranslator>().InternalCache(),
				$"{cacheDetails.ResponseTime:0.0}ms",
				false
			);

		if (!(botDetails.Online && dbDetails.Online && cacheDetails.Online))
			embed.WithColor(Color.Red);

		StringBuilder loggedInString = new();
		var loggedInCount = 0;

		foreach (var item in IdentityManager.GetCurrentIdentities().Where(x => x is DiscordOAuthIdentity))
		{
			var user = item.GetCurrentUser();

			if (user != null)
			{
				loggedInString.AppendLine($"{user.Username}#{user.Discriminator}");
				loggedInCount++;
			}
		}

		if (loggedInCount != 0)
			embed.AddField(
				$"{Translator.Get<UtilityTranslator>().CurrentlyLoggedIn()} [{loggedInCount}]",
				loggedInString.ToString().Truncate(1024),
				false
			);

		await Context.Interaction.ModifyOriginalResponseAsync((msg) => { msg.Embed = embed.Build(); });
	}
}
