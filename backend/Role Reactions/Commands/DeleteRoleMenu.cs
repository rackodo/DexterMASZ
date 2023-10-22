using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;
using System.Runtime.InteropServices;

namespace RoleReactions.Commands;

public class DeleteRoleMenu : RoleMenuCommand<DeleteRoleMenu>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("delete-rm", "Deletes a menu that users can pick their roles from!")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RoleMenuCommand(string title, ITextChannel channel = null)
    {
        if (channel == null)
            if (Context.Channel is ITextChannel txtChannel)
                channel = txtChannel;

        if (channel != null)
        {
            var menu = Database.RoleReactionsMenu.Find(channel.Id, channel.GuildId, title);

            if (menu == null)
            {
                await RespondInteraction($"Role menu {title} does not exist in this channel!");
                return;
            }

            Database.Remove(menu);
            await channel.DeleteMessageAsync(menu.MessageId);

            await Database.SaveChangesAsync();

            await RespondInteraction($"Role menu {title} is now deleted!");
        }
        else
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }
    }
}
