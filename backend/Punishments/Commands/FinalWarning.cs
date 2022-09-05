using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using Punishments.Abstractions;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Exceptions;
using Punishments.Models;

namespace Punishments.Commands;

public class FinalWarning : PunishmentCommand<FinalWarning>
{
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
		if (await ModCaseRepository.GetFinalWarn(user.Id, Context.Guild.Id) != null)
			throw new AlreadyFinalWarnedException();

		PunishmentConfigRepository.AsUser(Identity);

		var punishmentConfig = await PunishmentConfigRepository.GetGuildPunishmentConfig(Context.Guild.Id);

		await RunModcase(new ModCase()
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
		});
	}
}
