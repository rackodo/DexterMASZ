using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;

namespace RoleReactions.Commands;

public class DeleteRoleMenu : RoleMenuCommand<DeleteRoleMenu>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("delete-rm", "Deletes a menu that users can pick their roles from!")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task DeleteRoleMenuCommand([Autocomplete(typeof(MenuHandler))] string menuStr, ITextChannel channel = null)
    {
        if (channel == null)
            if (Context.Channel is ITextChannel txtChannel)
                channel = txtChannel;

        if (channel == null)
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }

        var menuArray = menuStr.Split(',');
        var menuId = int.Parse(menuArray[0]);
        var channelId = ulong.Parse(menuArray[1]);

        var menu = Database.RoleReactionsMenu.Find(channel.GuildId, channel.Id, menuId);

        if (menu == null)
        {
            await RespondInteraction($"Role menu `{menuId}` does not exist in this channel!");
            return;
        }

        if (channelId != channel.Id)
        {
            await RespondInteraction($"The role menu {menu.Name} does not match the channel {channel.Name}!");
            return;
        }

        var message = await channel.GetMessageAsync(menu.MessageId);

        if (message != null)
            await channel.DeleteMessageAsync(menu.MessageId);

        Database.RoleReactionsMenu.Remove(menu);

        var addedRoles = Database.UserRoles.Where(
            x => x.GuildId == Context.Guild.Id &&
            x.ChannelId == Context.Channel.Id &&
            x.Id == menuId
        );

        foreach (var role in addedRoles)
            Database.UserRoles.Remove(role);

        await Database.SaveChangesAsync();

        await RespondInteraction($"Role menu `{menu.Name}` is now deleted!");
    }
}
