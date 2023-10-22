using Bot.Abstractions;
using Discord.Interactions;

namespace RoleReactions.Abstractions;

[Group("music", "Commands to add a role menu to your server!")]
public class RoleMenuCommand<T> : Command<T>
{
    public override async Task BeforeCommandExecute() =>
        await Context.Interaction.DeferAsync(true);
}
