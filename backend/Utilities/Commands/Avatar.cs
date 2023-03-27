using Bot.Abstractions;
using Bot.Attributes;
using Bot.Extensions;
using Bot.Services;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Utilities.Commands;

public class Avatar : Command<Avatar>
{
    public DiscordRest Client { get; set; }

    [SlashCommand("avatar", "Get the high resolution avatar of a user.")]
    [BotChannel]
    public async Task AvatarCommand([Summary("user", "User to get the avatar from")] IUser user = null)
    {
        user ??= Context.User;
        await Context.Interaction.DeferAsync();
        await UserAvatar(user.Id.ToString(), true);
    }

    [ComponentInteraction("avatar-user:*,*")]
    public async Task UserAvatar(string userId, bool isGuild)
    {
        var id = ulong.Parse(userId);

        var user = await Client.GetRestClient().GetUserAsync(id);
        var gUser = await Client.GetRestClient().GetGuildUserAsync(Context.Guild.Id, id);

        var guildAvail = gUser != null;

        var guildAvatar = string.Empty;

        if (guildAvail)
        {
            guildAvatar = gUser.GetGuildAvatarUrl(size: 1024);

            if (string.IsNullOrEmpty(guildAvatar))
                guildAvail = false;
        }

        if (isGuild && !guildAvail)
            isGuild = false;

        var avatarUrl = isGuild ? guildAvatar : user.GetAvatarOrDefaultUrl(size: 1024);

        var embed = new EmbedBuilder()
            .WithTitle((isGuild ? "Guild" : "User") + " Avatar URL")
            .WithFooter($"{Translator.Get<BotTranslator>().UserId()}: {(gUser ?? user).Id}")
            .WithUrl(avatarUrl)
            .WithImageUrl(avatarUrl)
            .WithAuthor(gUser ?? user)
            .WithColor(Color.Magenta)
            .WithCurrentTimestamp();

        var buttons = new ComponentBuilder();

        if (guildAvail)
            buttons.WithButton($"Get {(isGuild ? "User" : "Guild")} Avatar", $"avatar-user:{user.Id},{!isGuild}");

        if (Context.Interaction is SocketMessageComponent castInteraction)
            await castInteraction.UpdateAsync(message =>
            {
                message.Embed = embed.Build();
                message.Components = buttons.Build();
            });
        else
            await Context.Interaction.ModifyOriginalResponseAsync(message =>
            {
                message.Embed = embed.Build();
                message.Components = buttons.Build();
            });
    }
}
