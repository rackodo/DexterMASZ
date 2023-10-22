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
    public async Task RoleMenuCommand([Autocomplete(typeof(MenuHandler))] int menuId, ITextChannel channel = null)
    {
        if (channel == null)
            if (Context.Channel is ITextChannel txtChannel)
                channel = txtChannel;

        if (channel != null)
        {
            var menu = Database.RoleReactionsMenu.Find(channel.GuildId, channel.Id, menuId);

            if (menu == null)
            {
                await RespondInteraction($"Role menu `{menuId}` does not exist in this channel!");
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

            Database.SaveChanges();

            await RespondInteraction($"Role menu `{menu.Name}` is now deleted!");
        }
        else
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }
    }
}
