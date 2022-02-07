using Discord;
using Discord.Interactions;
using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Models;
using Punishments.Translators;

namespace Punishments.Commands;

public class Warn : Command<Warn>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("warn", "Warn a user and create a mod case")]
	public async Task WarnCommand(
		[Summary("title", "The title of the mod case")]
		string title,
		[Summary("user", "User to punish")] IUser user,
		[Summary("description", "The description of the mod case")]
		string description = "",
		[Summary("dm-notification", "Whether to send a dm notification")]
		bool sendDmNotification = true,
		[Summary("public-notification", "Whether to send a public webhook notification")]
		bool sendPublicNotification = true)
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
			PunishmentType = PunishmentType.Warn,
			PunishmentActive = true,
			PunishedUntil = null,
			CreationType = CaseCreationType.ByCommand
		};

		var created = await ModCaseRepository.CreateModCase(modCase, true, sendPublicNotification, sendDmNotification);

		var config = await SettingsRepository.GetAppSettings();

		var url = $"{config.ServiceBaseUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}