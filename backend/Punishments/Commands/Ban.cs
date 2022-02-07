using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Models;
using Punishments.Translators;

namespace Punishments.Commands;

public class Ban : Command<Ban>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeBan)]
	[SlashCommand("ban", "Ban a user and create a mod case")]
	public async Task BanCommand(
		[Summary("title", "The title of the mod case")]
		string title,
		[Summary("user", "User to punish")] IUser user,
		[Choice("None", 0)]
		[Choice("1 Hour", 1)]
		[Choice("1 Day", 24)]
		[Choice("1 Week", 168)]
		[Choice("1 Month", 672)]
		[Summary("hours", "How long the punishment should be")]
		long punishedForHours = 0,
		[Summary("description", "The description of the mod case")]
		string description = "",
		[Summary("dm-notification", "Whether to send a dm notification")]
		bool sendDmNotification = true,
		[Summary("public-notification", "Whether to send a public webhook notification")]
		bool sendPublicNotification = true,
		[Summary("execute-punishment", "Whether to execute the punishment or just register it.")]
		bool executePunishment = true)
	{
		ModCaseRepository.AsUser(Identity);

		await Context.Interaction.DeferAsync(ephemeral: !sendPublicNotification);

		ModCase modCase = new()
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Ban,
			PunishmentActive = executePunishment,
			PunishedUntil = DateTime.UtcNow.AddHours(punishedForHours)
		};

		// Permanent ban.
		if (punishedForHours == 0)
			modCase.PunishedUntil = null;

		modCase.CreationType = CaseCreationType.ByCommand;

		var created =
			await ModCaseRepository.CreateModCase(modCase, executePunishment, sendPublicNotification,
				sendDmNotification);

		var url =
			$"{(await SettingsRepository.GetAppSettings()).ServiceBaseUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}