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

            Database.Remove(menu);
            await channel.DeleteMessageAsync(menu.MessageId);

            await Database.SaveChangesAsync();

            await RespondInteraction($"Role menu `{menu.Name}` is now deleted!");
        }
        else
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }
    }
}
