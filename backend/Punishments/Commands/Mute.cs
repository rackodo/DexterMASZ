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

public class Mute : Command<Mute>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeMute)]
	[SlashCommand("mute", "Mute a user and create a modcase")]
	public async Task MuteCommand(
		[Summary("title", "The title of the modcase")]
		string title,
		[Summary("user", "User to punish")] IUser user,
		[Choice("None", 0)]
		[Choice("1 Hour", 1)]
		[Choice("1 Day", 24)]
		[Choice("1 Week", 168)]
		[Choice("1 Month", 672)]
		[Summary("hours", "How long the punishment should be")]
		long punishedForHours = 0,
		[Summary("description", "The description of the modcase")]
		string description = "",
		[Summary("Severity Level", "Whether or not this is a higher severity case")]
		bool highSeverity = false,
		[Summary("execute-punishment", "Whether to execute the punishment or just register it")]
		bool executePunishment = true)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Channel.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var modCase = new ModCase()
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Mute,
			PunishmentActive = executePunishment,
			HighSeverity = highSeverity,
			PunishedUntil = DateTime.UtcNow.AddHours(punishedForHours)
		};

		// Remove duration for permanent mute
		if (punishedForHours == 0)
			modCase.PunishedUntil = null;

		modCase.CreationType = CaseCreationType.ByCommand;

		var created =
			await ModCaseRepository.CreateModCase(modCase, executePunishment);

		var config = await SettingsRepository.GetAppSettings();

		var url = $"{config.ServiceBaseUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}