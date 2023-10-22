using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;
using RoleReactions.Models;
using System.Runtime.InteropServices;

namespace RoleReactions.Commands;

public class RemoveAssignedRole : RoleMenuCommand<RemoveAssignedRole>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("removeRole", "Removes a role to a role menu")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RemoveAssignedRoleCommand(string menuName, string roleName,
        [Optional] IMessageChannel channel)
    {
        channel ??= Context.Channel;

        if (channel is ITextChannel txtChannel)
        {
            var menu = Database.RoleReactionsMenu.Find(txtChannel.Id, txtChannel.GuildId, menuName);

            if (menu == null)
            {
                await RespondInteraction($"Role menu {menuName} does not exist in this channel!");
                return;
            }

            if (!menu.Roles.ContainsKey(roleName))
            {
                await RespondInteraction($"Role {roleName} does not exist for role menu {menuName}!");
                return;
            }

            var message = await txtChannel.GetMessageAsync(menu.MessageId);

            if (message == null)
            {
                await RespondInteraction($"Role menu {menuName} does not have a message related to it! " +
                    $"Please delete and recreate the menu.");
                return;
            }

            if (message is IUserMessage userMessage)
            {
                var rows = new List<List<AssignedRole>>();
                var tempComp = new List<AssignedRole>();

                foreach (var storeRole in menu.Roles)
                {
                    if (storeRole.Key == roleName)
                        continue;

                    tempComp.Add(new AssignedRole()
                    {
                        RoleName = storeRole.Key,
                        RoleId = storeRole.Value,
                        Emote = menu.Emotes[storeRole.Key]
                    });

                    if (tempComp.Count >= 5)
                        rows.Add(tempComp);
                }

                rows.Add(tempComp);

                var components = new ComponentBuilder();

                foreach (var row in rows)
                {
                    var aRow = new ActionRowBuilder();

                    foreach (var col in row)
                    {
                        IEmote intEmote = null;

                        if (Emote.TryParse(col.Emote, out var pEmote))
                            intEmote = pEmote;

                        aRow.WithButton(col.RoleName, $"add-role:{col.RoleId},{Context.User.Id}", emote: intEmote);
                    }

                    components.AddRow(aRow);
                }

                await userMessage.ModifyAsync(m => m.Components = components.Build());

                menu.Roles.Remove(roleName);
                menu.Emotes.Remove(roleName);

                await Database.SaveChangesAsync();

                await RespondInteraction($"Successfully removed role {roleName} from menu {menuName}!");
            }
            else
            {
                await RespondInteraction($"Message for role menu {menuName} was not created by me! Please delete and recreate the menu.");
                return;
            }
        }
        else
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }
    }
}
