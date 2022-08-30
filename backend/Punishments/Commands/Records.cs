using Bot.Abstractions;
using Bot.Data;
using Discord;
using Discord.Interactions;
using Punishments.Data;
using Punishments.Enums;
using Punishments.Models;
using Punishments.Translators;

namespace Punishments.Commands;

public class Records : Command<Records>
{
	public ModCaseRepository ModCaseRepository { get; set; }
	public SettingsRepository SettingsRepository { get; set; }
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[SlashCommand("records", "Gets the records for the current user.")]
	public async Task RecordsCommand()
	{
		var appSettings = await SettingsRepository.GetAppSettings();
		await Context.Interaction.RespondAsync($"For your mod case records, please visit: {appSettings.GetServiceUrl()}/guilds/{Context.Guild.Id}", ephemeral: true);
	}
}