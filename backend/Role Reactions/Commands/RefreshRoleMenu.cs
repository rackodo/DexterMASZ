using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;

namespace RoleReactions.Commands;

public class RefreshRoles : RoleMenuCommand<RefreshRoles>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("refresh-rm-roles", "Recreates the buttons on a role menu.")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RefreshRolesCommand([Autocomplete(typeof(MenuHandler))] int menuId,
        ITextChannel channel = null)
    {
        if (channel == null)
            if (Context.Channel is ITextChannel txtChannel)
                channel = txtChannel;

        if (channel == null)
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }

        var menu = Database.RoleReactionsMenu.Find(channel.GuildId, channel.Id, menuId);

        if (menu == null)
        {
            await RespondInteraction($"Role menu `{menuId}` does not exist in this channel!");
            return;
        }

        var message = await channel.GetMessageAsync(menu.MessageId);

        if (message == null)
        {
            await RespondInteraction($"Role menu `{menu.Name}` does not have a message related to it! " +
                $"Please delete and recreate the menu.");
            return;
        }

        if (message is not IUserMessage userMessage)
        {
            await RespondInteraction($"Message for role menu `{menu.Name}` was not created by me! " +
                $"Please delete and recreate the menu.");
            return;
        }

        var embed = userMessage.Embeds.FirstOrDefault();

        if (embed == null)
        {
            await RespondInteraction($"Embed for role menu `{menu.Name}` could not be found!");
            return;
        }

        var embedBuilder = embed.ToEmbedBuilder();
        ApplyMenuData(menu, embedBuilder);
        var newEmbed = embedBuilder.Build();
        await userMessage.ModifyAsync(m => m.Embed = newEmbed);

        await CreateRoleMenu(menu, userMessage);

        await RespondInteraction($"Successfully refreshed buttons on role menu `{menu.Name}`!");
    }
}
