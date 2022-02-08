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

public class Kick : Command<Kick>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator, RequireCheck.GuildStrictModeKick)]
	[SlashCommand("kick", "Kick a user and create a modcase")]
	public async Task KickCommand(
		[Summary("title", "The title of the modcase")]
		string title,
		[Summary("user", "User to punish")] IUser user,
		[Summary("description", "The description of the modcase")]
		string description = "",
		[Summary("dm-notification", "Whether to send a dm notification")]
		bool sendDmNotification = true,
		[Summary("execute-punishment", "Whether to execute the punishment or just register it.")]
		bool executePunishment = true)
	{
		ModCaseRepository.AsUser(Identity);
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Channel.Id);

		await Context.Interaction.DeferAsync(ephemeral: !guildConfig.StaffChannels.Contains(Context.Channel.Id));

		var modCase = new ModCase
		{
			Title = title,
			GuildId = Context.Guild.Id,
			UserId = user.Id,
			ModId = Identity.GetCurrentUser().Id,
			Description = string.IsNullOrEmpty(description) ? title : description,
			PunishmentType = PunishmentType.Kick,
			PunishmentActive = executePunishment,
			PunishedUntil = null,
			CreationType = CaseCreationType.ByCommand
		};

		var created =
			await ModCaseRepository.CreateModCase(modCase, executePunishment,
				sendDmNotification);

		var config = await SettingsRepository.GetAppSettings();

		var url = $"{config.ServiceBaseUrl}/guilds/{created.GuildId}/cases/{created.CaseId}";

		await Context.Interaction.ModifyOriginalResponseAsync((MessageProperties msg) =>
		{
			msg.Content = Translator.Get<PunishmentTranslator>().CaseCreated(created.CaseId, url);
		});
	}
}