using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Discord.Interactions;

namespace DexterSlash.Commands.UtilityCommands;

public class Latency : Command<Latency>
{
	public IServiceProvider ServiceProvider { get; set; }

	[SlashCommand("latency", "Gets the estimate round-trip latency to the gateway server.")]
	public async Task LatencyCommand()
	{
		var embed = (await EmbedCreator.CreateActionEmbed(RestAction.Created, ServiceProvider))
			.WithTitle("Gateway Ping")
			.WithDescription($"Current latency:\n**{Context.Client.Latency}ms**");

		await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
	}
}
