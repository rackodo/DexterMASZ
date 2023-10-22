using Bot.Abstractions;

namespace RoleReactions.Abstractions;

public class RoleMenuCommand<T> : Command<T>
{
    public override async Task BeforeCommandExecute() =>
        await Context.Interaction.DeferAsync(true);
}
