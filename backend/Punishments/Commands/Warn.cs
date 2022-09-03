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

public class Warn : Command<Warn>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("warn", "Warn a user and create a mod case")]
	public async Task WarnCommand(
		[Summary("title", "The title of the modcase")]
		string title,
		[Summary("user", "User to punish")]
		IUser user,
		[Summary("severity-level", "Whether or not this is a higher or lower severity case")]
		InnerSeverityType severity,
		[Summary("description", "The description of the modcase")]
		string description = "")
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var modCase = new ModCase()
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Warn,
			PunishmentActive = true,
			PunishedUntil = null,
			Severity = (SeverityType) severity,
			CreationType = CaseCreationType.ByCommand
		};

		var created = await ModCaseRepository.CreateModCase(modCase);

		var config = await SettingsRepository.GetAppSettings();

		var url = $"{config.GetServiceUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		var caseCount =
			(await ModCaseRepository.GetCasesForGuildAndUser(Context.Guild.Id, user.Id)).Count;

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url, caseCount);
		});
	}
}