using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Bot.Extensions;
using Discord.Interactions;

namespace Utilities.Commands;

public class Emote : Command<Emote>
{
    public IServiceProvider ServiceProvider { get; set; }

    public override async Task BeforeCommandExecute() =>
        await Context.Interaction.DeferAsync(true);

    [SlashCommand("emote", "Gets the full image of an emote.")]
    [BotChannel]
    public async Task EmoteCommand(
        [Summary("emote", "The Discord emote you wish to be enlargened!")]
        string emote)
    {
        var embed = Discord.Emote.TryParse(emote, out var emojis)
            ? (await EmbedCreator.CreateActionEmbed(RestAction.Created, ServiceProvider))
                .WithImageUrl(emojis.Url)
                .WithUrl(emojis.Url)
                .WithAuthor(emojis.Name)
                .WithTitle("Get Emoji URL")
            : (await EmbedCreator.CreateActionEmbed(RestAction.Deleted, ServiceProvider))
                .WithTitle("Unknown Emoji")
                .WithDescription(
                    "An invalid emoji was specified! Please make sure that what you have sent is a valid emoji. " +
                    "Please make sure this is a **custom emoji** aswell, and that it does not fall under the unicode specification.");

        await RespondInteraction(string.Empty, embed);
    }
}
