using Bot.Abstractions;
using Bot.Extensions;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Utilities.Translators;

namespace Utilities.Commands;

public class Avatar : Command<Avatar>
{
    [SlashCommand("avatar", "Get the high resolution avatar of a user.")]
    public async Task AvatarCommand([Summary("user", "User to get the avatar from")] IUser user = null)
    {
        user ??= Context.User;
        await Context.Interaction.DeferAsync();
        await UserAvatar(user.Id.ToString(), true);
    }

    [ComponentInteraction("avatar-user:*,*")]
    public async Task UserAvatar(string userId, bool isGuild)
    {
        IUser user = Context.Client.GetUser(ulong.Parse(userId));
        IGuildUser gUser = Context.Guild.GetUser(ulong.Parse(userId));
        var guildAvail = false;

        if (gUser is { GuildAvatarId: { } })
            guildAvail = true;

        if (isGuild && !guildAvail)
            isGuild = false;

        var avatarUrl = isGuild ? gUser.GetGuildAvatarUrl(size: 1024) : user.GetAvatarOrDefaultUrl(size: 1024);
        var translator = Translator.Get<UtilityTranslator>();

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
