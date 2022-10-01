using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using Punishments.Abstractions;
using Punishments.Enums;
using Punishments.Models;
using System.Threading;

namespace Punishments.Commands;

public class Mute : PunishmentCommand<Mute>
{
	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeMute)]
	[SlashCommand("mute", "Mute a user and create a modcase")]
	public async Task MuteCommand(
		[Summary("title", "The title of the modcase")]
		string title,
		[Summary("user", "User to punish")]
		IUser user,
		[Summary("severity-level", "Whether or not this is a higher or lower severity case")]
		InnerSeverityType severity,
		[Summary("description", "The description of the modcase")]
		string description = "",
		[Summary("time", "The time to punish the user for")]
		TimeSpan time = default)
	{
		await RunModcase(new ModCase
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Mute,
			PunishmentActive = true,
			Severity = (SeverityType) severity,
			PunishedUntil = time == default ? null : time + DateTime.UtcNow,
			CreationType = CaseCreationType.ByCommand
		});
	}
}
