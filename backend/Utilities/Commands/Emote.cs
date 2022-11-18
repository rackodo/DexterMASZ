using Bot.Abstractions;
using Bot.Enums;
using Bot.Extensions;
using Discord;
using Discord.Interactions;

namespace Utilities.Commands;

public class Emote : Command<Emote>
{
    public IServiceProvider ServiceProvider { get; set; }

    [SlashCommand("emote", "Gets the full image of an emote.")]
    public async Task EmoteCommand(
        [Summary("emote", "The Discord emote you wish to be enlargened!")]
        string emoji)
    {
        EmbedBuilder embed;

        if (Discord.Emote.TryParse(emoji, out var emojis))
            embed = (await EmbedCreator.CreateActionEmbed(RestAction.Created, ServiceProvider))
                .WithImageUrl(emojis.Url)
                .WithUrl(emojis.Url)
                .WithAuthor(emojis.Name)
                .WithTitle("Get Emoji URL");
        else
            embed = (await EmbedCreator.CreateActionEmbed(RestAction.Deleted, ServiceProvider))
                .WithTitle("Unknown Emoji")
                .WithDescription(
                    "An invalid emoji was specified! Please make sure that what you have sent is a valid emoji. " +
                    "Please make sure this is a **custom emoji** aswell, and that it does not fall under the unicode specification.");

        await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
    }
}