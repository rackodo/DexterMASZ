using Discord.Interactions;
using MASZ.Bot.Abstractions;
using MASZ.Utilities.Translators;

namespace MASZ.Utilities.Commands;

public class Invite : Command<Invite>
{
	[SlashCommand("invite", "How to invite this bot.")]
	public async Task InviteCommand()
	{
		await Context.Interaction.RespondAsync(Translator.Get<UtilityTranslator>().Invite());
	}
}