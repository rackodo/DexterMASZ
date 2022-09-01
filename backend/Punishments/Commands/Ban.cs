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
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeBan)]
	[SlashCommand("ban", "Ban a user and create a mod case")]
	public async Task BanCommand(
		[Summary("title", "The title of the mod case")]
		string title,
		[Summary("user", "User to punish")]
		IUser user,
		[Summary("description", "The description of the mod case")]
		string description = "",
		[Summary("time", "The time to punish the user for")]
		TimeSpan time = default)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var modCase = new ModCase
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Ban,
			PunishmentActive = true,
			PunishedUntil = time == default ? null : DateTime.UtcNow + time,
			CreationType = CaseCreationType.ByCommand,
			Severity = SeverityType.High
		};

		var created =
			await ModCaseRepository.CreateModCase(modCase);

		var url =
			$"{(await SettingsRepository.GetAppSettings()).GetServiceUrl()}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}