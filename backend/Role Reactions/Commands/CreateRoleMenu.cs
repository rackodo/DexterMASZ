using Bot.Abstractions;
using Bot.Attributes;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using System.Runtime.InteropServices;

namespace RoleReactions.Commands;

public class CreateRoleMenu : Command<CreateRoleMenu>
{

    [SlashCommand("createrolemenu", "Create a menu that users can pick their roles from!")]
    [Require(RequireCheck.GuildAdmin)]
    public async Task RoleMenuCommand(string title, string description,
        [Optional] IMessageChannel channel, [Optional] string colorHex)
    {
        var color = Color.Teal;

        if (!string.IsNullOrEmpty(colorHex))
            color = new Color(
                Convert.ToUInt32(colorHex.ToUpper().Replace("#", ""), 16)
            );

        var embed = new EmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(color);

        channel ??= Context.Channel;

        var msg = await channel.SendMessageAsync(embed: embed.Build());

        
    }
}
