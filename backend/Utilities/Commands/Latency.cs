using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Bot.Extensions;
using Discord.Interactions;

namespace Utilities.Commands;

public class Latency : Command<Latency>
{
    public IServiceProvider ServiceProvider { get; set; }

    [SlashCommand("latency", "Gets the estimate round-trip latency to the gateway server.")]
    [BotChannel]
    public async Task LatencyCommand()
    {
        var embed = (await EmbedCreator.CreateActionEmbed(RestAction.Created, ServiceProvider))
            .WithTitle("Gateway Ping")
            .WithDescription($"Current latency:\n**{Context.Client.Latency}ms**");

        await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
    }
}
