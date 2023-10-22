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

public class AddAssignedRole : RoleMenuCommand<AddAssignedRole>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("assignRole", "Assigns a role to a role menu")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task AddAssignedRoleCommand(string emote, string menuName, IRole role,
        string name, [Optional] IMessageChannel channel)
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

            if (menu.Roles.ContainsKey(name))
            {
                await RespondInteraction($"Role {name} already exists for role menu {menuName}!");
                return;
            }

            var message = await txtChannel.GetMessageAsync(menu.MessageId);

            if (message == null)
            {
                await RespondInteraction($"Role menu {menuName} does not have a message related to it! " +
                    $"Please delete and recreate the menu.");
                return;
            }

            if (!Emote.TryParse(emote, out var _))
            {
                await RespondInteraction($"Emote {emote} could not be found!");
                return;
            }

            if (message is IUserMessage userMessage)
            {
                var rows = new List<List<AssignedRole>>();
                var tempComp = new List<AssignedRole>();

                var count = 1;

                foreach (var storeRole in menu.Roles)
                {
                    tempComp.Add(new AssignedRole()
                    {
                        RoleName = storeRole.Key,
                        RoleId = storeRole.Value,
                        Emote = menu.Emotes[storeRole.Key]
                    });

                    if (tempComp.Count >= 5)
                        rows.Add(tempComp);

                    count++;
                }

                if (count > 25)
                {
                    await RespondInteraction($"Too many roles in manu {menuName}! Please create a new role menu.");
                    return;
                }

                tempComp.Add(new AssignedRole()
                {
                    RoleName = name,
                    RoleId = role.Id,
                    Emote = emote
                });

                rows.Add(tempComp);

                var components = new ComponentBuilder();

                foreach(var row in rows)
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

                menu.Roles.Add(name, role.Id);
                menu.Emotes.Add(name, emote);

                await Database.SaveChangesAsync();

                await RespondInteraction($"Successfully added role {role.Name} to menu {menuName}!");
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

    [ComponentInteraction("add-role:*,*")]
    public async Task AddRole(string sRoleId, string sUserId)
    {
        var userId = ulong.Parse(sUserId);
        var roleId = ulong.Parse(sRoleId);

        var user = Context.Guild.GetUser(userId);
        var userRole = user.Roles.FirstOrDefault(r => r.Id == roleId);

        var userInfo = Database.UserRoles.Find(Context.Guild.Id, userId);

        if (userInfo == null)
        {
            userInfo = new UserRoles()
            {
                GuildId = Context.Guild.Id,
                UserId = userId,
                RoleIds = new List<ulong>()
            };

            Database.UserRoles.Add(userInfo);
        }

        if (userRole != null)
        {
            await user.RemoveRoleAsync(userRole);

            var embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithTitle("Removed Role")
                .WithDescription($"{userRole.Mention} has been removed from {user.Mention}!")
                .WithCurrentTimestamp();

            if (userInfo.RoleIds.Contains(roleId))
                userInfo.RoleIds.Remove(roleId);

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }
        else
        {
            var role = Context.Guild.GetRole(roleId);
            await user.AddRoleAsync(userRole);

            if (!userInfo.RoleIds.Contains(roleId))
                userInfo.RoleIds.Add(roleId);

            var embed = new EmbedBuilder()
                .WithColor(Color.Green)
                .WithTitle("Added Role")
                .WithDescription($"{role.Mention} has been added to {user.Mention}!")
                .WithCurrentTimestamp();

            await RespondAsync(embed: embed.Build(), ephemeral: true);
        }

        await Database.SaveChangesAsync();
    }
}
