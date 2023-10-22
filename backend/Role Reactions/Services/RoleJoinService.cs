using Bot.Abstractions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RoleReactions.Data;

namespace RoleReactions.Services;

public class RoleJoinService(DiscordSocketClient client, IServiceProvider serviceProvider) : IEvent
{
    public void RegisterEvents() => client.UserJoined += AddRoles;

    private async Task AddRoles(SocketGuildUser user)
    {
        using var scope = serviceProvider.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<RoleReactionsDatabase>();

        var userInfo = database.UserRoles.Find(user.Guild.Id, user.Id);

        if (userInfo == null)
            return;

        foreach (var roleId in userInfo.RoleIds)
        {
            var role = user.Guild.GetRole(roleId);

            if (role == null) continue;

            await user.AddRoleAsync(role);
        }
    }
}
