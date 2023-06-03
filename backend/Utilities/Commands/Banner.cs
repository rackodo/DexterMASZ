using Bot.Abstractions;
using Bot.Attributes;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;

namespace Utilities.Commands;

public class Banner : Command<Banner>
{
    public DiscordRest Client { get; set; }

    [SlashCommand("banner", "Get the high resolution banner of a user.")]
    [BotChannel]
    public async Task BannerCommand([Summary("user", "User to get the banner from")] IUser user = null)
    {
        user ??= Context.User;
        await UserBanner(user.Id.ToString(), true);
    }

    [ComponentInteraction("banner-user:*,*")]
    public async Task UserBanner(string userId, bool isGuild)
    {
        var id = ulong.Parse(userId);

        var user = await Client.GetRestClient().GetUserAsync(id);
        var gUser = await Client.GetRestClient().GetGuildUserAsync(Context.Guild.Id, id);

        var guildAvail = gUser != null;

        if (isGuild && !guildAvail)
            isGuild = false;

        var avatarUrl = isGuild ? gUser.GetBannerUrl(size: 1024) : user.GetBannerUrl(size: 1024);

        var embed = new EmbedBuilder()
            .WithTitle((isGuild ? "Guild" : "User") + " Banner URL")
            .WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {(gUser ?? user).Id}")
            .WithUrl(avatarUrl)
            .WithImageUrl(avatarUrl)
            .WithAuthor(gUser ?? user)
            .WithColor(Color.Magenta)
            .WithCurrentTimestamp();

        var buttons = new ComponentBuilder();

        if (guildAvail)
            buttons.WithButton($"Get {(isGuild ? "User" : "Guild")} Banner", $"banner-user:{user.Id},{!isGuild}");

        await RespondInteraction(string.Empty, embed, buttons);
    }
}
