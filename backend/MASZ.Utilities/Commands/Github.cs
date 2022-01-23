using Discord.Interactions;
using MASZ.Bot.Abstractions;

namespace MASZ.Utilities.Commands;

public class GitHub : Command<GitHub>
{
	[SlashCommand("github", "Displays the GitHub repository URL.")]
	public async Task GitHubCommand()
	{
		await Context.Interaction.RespondAsync("https://github.com/zaanposni/discord-MASZ");
	}
}