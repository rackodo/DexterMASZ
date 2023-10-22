using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;
using RoleReactions.Models;

namespace RoleReactions.Commands;

public class AddAssignedRole : RoleMenuCommand<AddAssignedRole>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("add-rm-role", "Assigns a role to a role menu")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task AddAssignedRoleCommand(string emote, [Autocomplete(typeof(MenuHandler))] int menuId,
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

            if (menu.RoleToEmote.ContainsKey(role.Id))
            {
                await RespondInteraction($"Role `{role.Name}` already exists for role menu `{menu.Name}`!");
                return;
            }

            var message = await channel.GetMessageAsync(menu.MessageId);

            if (message == null)
            {
                await RespondInteraction($"Role menu `{menu.Name}` does not have a message related to it! " +
                    $"Please delete and recreate the menu.");
                return;
            }

            if (!Emote.TryParse(emote, out var _) && !Emoji.TryParse(emote, out var _))
            {
                await RespondInteraction($"Emote `{emote}` could not be found!");
                return;
            }

            if (message is IUserMessage userMessage)
            {
                var rows = new List<Dictionary<ulong, string>>();
                var tempComp = new Dictionary<ulong, string>();

                var count = 1;

                foreach (var storeRole in menu.RoleToEmote)
                {
                    tempComp.Add(storeRole.Key, storeRole.Value);

                    if (tempComp.Count >= 5)
                        rows.Add(tempComp);

                    count++;
                }

                if (count > 25)
                {
                    await RespondInteraction($"Too many roles in manu `{menu.Name}`! " +
                        $"Please create a new role menu.");
                    return;
                }

                tempComp.Add(role.Id, emote);

                rows.Add(tempComp);

                var components = new ComponentBuilder();

                foreach(var row in rows)
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

                menu.RoleToEmote.Add(role.Id, emote);

                await Database.SaveChangesAsync();

                await RespondInteraction($"Successfully added role `{role.Name}` to menu `{menu.Name}`!");
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

    [ComponentInteraction("add-rm-role:*,*,*")]
    public async Task AddRole(string sRoleId, string sUserId, string sMenuId)
    {
        var roleId = ulong.Parse(sRoleId);
        var userId = ulong.Parse(sUserId);
        var menuId = int.Parse(sMenuId);

        var user = Context.Guild.GetUser(userId);
        var role = Context.Guild.GetRole(roleId);

        var userInfo = Database.UserRoles.Find(Context.Guild.Id, Context.Channel.Id, menuId, userId);

        if (userInfo == null)
        {
            userInfo = new UserRoles()
            {
                GuildId = Context.Guild.Id,
                ChannelId = Context.Channel.Id,
                Id = menuId,
                UserId = userId,
                RoleIds = new List<ulong>()
            };

            Database.UserRoles.Add(userInfo);
        }

        var embed = new EmbedBuilder()
                .WithCurrentTimestamp();

        if (user.Roles.Any(r => r.Id == role.Id))
        {
            await user.RemoveRoleAsync(role);

            embed
                .WithColor(Color.Red)
                .WithTitle("Removed Role")
                .WithDescription($"{role.Mention} has been removed from {user.Mention}!");

            userInfo.RoleIds.Remove(roleId);
        }
        else
        {
            await user.AddRoleAsync(role);

            if (!userInfo.RoleIds.Contains(roleId))
                userInfo.RoleIds.Add(roleId);

            embed
                .WithColor(Color.Green)
                .WithTitle("Added Role")
                .WithDescription($"{role.Mention} has been added to {user.Mention}!");
        }

        await Database.SaveChangesAsync();

        await FollowupAsync(embed: embed.Build(), ephemeral: true);
    }
}
