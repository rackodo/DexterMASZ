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

    [SlashCommand("deletemenu", "Deletes a menu that users can pick their roles from!")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RoleMenuCommand(string title, [Optional] IMessageChannel channel)
    {
        channel ??= Context.Channel;

        if (channel is ITextChannel txtChannel)
        {
            var menu = Database.RoleReactionsMenu.Find(txtChannel.Id, txtChannel.GuildId, title);

            if (menu == null)
            {
                await RespondInteraction($"Role menu {title} does not exist in this channel!");
                return;
            }

            Database.Remove(menu);
            await txtChannel.DeleteMessageAsync(menu.MessageId);

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
