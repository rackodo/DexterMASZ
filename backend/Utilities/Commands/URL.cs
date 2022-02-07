using Bot.Abstractions;
using Bot.Data;
using Discord.Interactions;

namespace Utilities.Commands;

public class UrlCommand : Command<UrlCommand>
{
	public SettingsRepository SettingsRepository { get; set; }

	[SlashCommand("url", "Displays the URL Dexter is deployed on.")]
	public async Task Url()
	{
		SettingsRepository.AsUser(Identity);

		await Context.Interaction.RespondAsync((await SettingsRepository.GetAppSettings()).ServiceBaseUrl);
	}
}