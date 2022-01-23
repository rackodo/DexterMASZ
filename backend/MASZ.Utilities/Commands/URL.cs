using Discord.Interactions;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;

namespace MASZ.Utilities.Commands;

public class UrlCommand : Command<UrlCommand>
{
	public SettingsRepository SettingsRepository { get; set; }

	[SlashCommand("url", "Displays the URL MASZ is deployed on.")]
	public async Task Url()
	{
		SettingsRepository.AsUser(Identity);

		await Context.Interaction.RespondAsync((await SettingsRepository.GetAppSettings()).ServiceBaseUrl);
	}
}