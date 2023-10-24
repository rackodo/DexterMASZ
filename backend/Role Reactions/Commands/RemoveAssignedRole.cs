using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;

namespace RoleReactions.Commands;

public class RemoveAssignedRole : RoleMenuCommand<RemoveAssignedRole>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("remove-rm-role", "Removes a role to a role menu")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RemoveAssignedRoleCommand([Autocomplete(typeof(MenuHandler))] string menuStr,
        IRole role, ITextChannel channel = null)
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

        if (!menu.RoleToEmote.ContainsKey(role.Id))
        {
            await RespondInteraction($"Role `{role.Name}` does not exist for role menu `{menu.Name}`!");
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
        
        menu.RoleToEmote.Remove(role.Id);
        await Database.SaveChangesAsync();

        await CreateRoleMenu(menu, userMessage);

        await RespondInteraction($"Successfully removed role `{role.Name}` from menu `{menu.Name}`!");
    }
}
