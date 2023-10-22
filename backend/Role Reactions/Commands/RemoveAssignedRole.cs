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
    public async Task RemoveAssignedRoleCommand([Autocomplete(typeof(MenuHandler))] int menuId,
        IRole role, ITextChannel channel = null)
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

            if (!menu.RoleToEmote.ContainsKey(role.Id))
            {
                await RespondInteraction($"Role `{role.Name}` does not exist for role menu `{menu.Name}`!");
                return;
            }

            var oldRoles = menu.RoleToEmote.ToDictionary(entry => entry.Key, entry => entry.Value);
            oldRoles.Remove(role.Id);
            menu.RoleToEmote = oldRoles;

            await Database.SaveChangesAsync();

            foreach (var roles in menu.RoleToEmote)
                Console.WriteLine(roles.Key + " " + roles.Value);

            var message = await channel.GetMessageAsync(menu.MessageId);

            if (message == null)
            {
                await RespondInteraction($"Role menu `{menu.Name}` does not have a message related to it! " +
                    $"Please delete and recreate the menu.");
                return;
            }

            if (message is IUserMessage userMessage)
            {
                var rows = new List<Dictionary<ulong, string>>();
                var tempComp = new Dictionary<ulong, string>();

                foreach (var storeRole in menu.RoleToEmote)
                {
                    if (storeRole.Key == role.Id)
                        continue;

                    tempComp.Add(storeRole.Key, storeRole.Value);

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

                        if (Emote.TryParse(col.Value, out var pEmote))
                            intEmote = pEmote;

                        if (Emoji.TryParse(col.Value, out var pEmoji))
                            intEmote = pEmoji;

                        var intRole = Context.Guild.GetRole(col.Key);

                        if (intRole != null)
                            aRow.WithButton(
                                intRole.Name,
                                $"add-rm-role:{intRole.Id},{Context.User.Id},{menu.Id}",
                                emote: intEmote
                            );
                    }

                    components.AddRow(aRow);
                }

                await userMessage.ModifyAsync(m => m.Components = components.Build());

                await RespondInteraction($"Successfully removed role `{role.Name}` from menu `{menu.Name}`!");
            }
            else
            {
                await RespondInteraction($"Message for role menu `{menu.Name}` was not created by me! " +
                    $"Please delete and recreate the menu.");
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
