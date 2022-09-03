using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Exceptions;
using Punishments.Models;
using Punishments.Translators;

namespace Punishments.Commands;

public class FinalWarning : Command<FinalWarning>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }
	public PunishmentConfigRepository PunishmentConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeBan)]
	[SlashCommand("finalwarn", "Issues a final warning to a user, mutes them and records the final warn.")]
	public async Task FinalWarnCommand(
		[Summary("title", "The title of the modcase")]
		string title,
		[Summary("user", "User to punish")]
		IUser user,
		[Summary("description", "The description of the modcase")]
		string description = "")
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);
		PunishmentConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);
		var punishmentConfig = await PunishmentConfigRepository.GetGuildPunishmentConfig(Context.Guild.Id);

		if (await ModCaseRepository.GetFinalWarn(user.Id, Context.Guild.Id) != null)
			throw new AlreadyFinalWarnedException();

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var modCase = new ModCase()
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.FinalWarn,
			PunishmentActive = true,
			PunishedUntil = punishmentConfig.FinalWarnMuteTime == default ? null : DateTime.UtcNow + punishmentConfig.FinalWarnMuteTime,
			Severity = SeverityType.None,
			CreationType = CaseCreationType.ByCommand
		};

		var created = await ModCaseRepository.CreateModCase(modCase);

		var config = await SettingsRepository.GetAppSettings();

		var url = $"{config.GetServiceUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}
