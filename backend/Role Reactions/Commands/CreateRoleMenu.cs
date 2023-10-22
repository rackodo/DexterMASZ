﻿using Bot.Attributes;
using Bot.Enums;
using Bot.Translators;
using Discord;
using Discord.Interactions;
using RoleReactions.Abstractions;
using RoleReactions.Data;
using RoleReactions.Models;

namespace RoleReactions.Commands;

public class CreateRoleMenu : RoleMenuCommand<CreateRoleMenu>
{
    public RoleReactionsDatabase Database { get; set; }

    [SlashCommand("create-rm", "Create a menu that users can pick their roles from!")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RoleMenuCommand(string title, string description,
        ITextChannel channel = null, string colorHex = default)
    {
        if (channel == null)
            if (Context.Channel is ITextChannel txtChannel)
                channel = txtChannel;

        if (channel != null)
        {
            var menu = Database.RoleReactionsMenu.Find(channel.GuildId, channel.Id, title);

            if (menu != null)
            {
                await RespondInteraction($"Role menu `{title}` already exists in this channel!");
                return;
            }

            var color = Color.Teal;

            if (!string.IsNullOrEmpty(colorHex))
                color = new Color(
                    Convert.ToUInt32(colorHex.ToUpper().Replace("#", ""), 16)
                );

            var embed = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(description)
                .WithColor(color);

            var msg = await channel.SendMessageAsync(embed: embed.Build());

            Database.RoleReactionsMenu.Add(
                new RoleMenu()
                {
                    ChannelId = channel.Id,
                    GuildId = channel.GuildId,
                    MenuName = title,
                    MessageId = msg.Id,
                    Roles = new Dictionary<string, ulong>(),
                    Emotes = new Dictionary<string, string>()
                }
            );

            await Database.SaveChangesAsync();

            await RespondInteraction($"Role menu `{title}` is now set up!");
        }
        else
        {
            await RespondInteraction(Translator.Get<BotTranslator>().OnlyTextChannel());
            return;
        }
    }
}
