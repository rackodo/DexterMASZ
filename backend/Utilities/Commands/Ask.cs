using Bot.Abstractions;
using Discord.Interactions;
using Genbox.WolframAlpha;
using System.ComponentModel.DataAnnotations;

namespace DexterSlash.Commands.UtilityCommands;

public class Ask : Command<Ask>
{
	public WolframAlphaClient WolframAlphaClient { get; set; }

	[SlashCommand("ask", "Evaluates mathematical expressions and answers questions!")]

	public async Task AskCommand(
		[Summary("question", "The question you wish answered!")]
		[MaxLength(1250)] string question)
	{
		var response = (await WolframAlphaClient.SpokenResultAsync(question))
			.Replace("Wolfram Alpha", Context.Client.CurrentUser.Username)
			.Replace("Wolfram|Alpha", Context.Client.CurrentUser.Username)
			.Replace("Stephen Wolfram", "the goat overlords")
			.Replace("and his team", "and their team");

		if (response.Contains("did not understand your input") || response.Contains("No spoken result available"))
			await RespondAsync(text: response, ephemeral: true);
		else
			await RespondAsync(text: $"**{question}**\n{response}");
	}
}
