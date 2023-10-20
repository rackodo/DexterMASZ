using Bot.Abstractions;
using Bot.Services;
using Discord.WebSocket;
using JoinLeave.Data;
using Microsoft.Extensions.DependencyInjection;

namespace JoinLeave.Services;

public class JoinLogger(DiscordSocketClient client, IServiceProvider services) : IEvent
{
    public void RegisterEvents()
    {
        client.UserJoined += MemberJoined;
        client.UserLeft += MemberLeft;
    }

    private async Task MemberJoined(SocketGuildUser user)
    {
        using var scope = services.CreateScope();

        var joinLeaveDb = scope.ServiceProvider.GetRequiredService<JoinLeaveDatabase>();

        var config = joinLeaveDb.JoinLeaveConfig.Find(user);

        if (config != null)
        {
            var channel = user.Guild.GetTextChannel(config.JoinChannelId);

            if (channel != null)
            {
                var message = config.JoinMessage
                    .Replace("{SERVERNAME}", user.Guild.Name)
                    .Replace("{USERNAME}", user.Username)
                    .Replace("{MENTION}", user.Mention);

                await channel.SendMessageAsync(message);
            }
        }
    }

    private async Task MemberLeft(SocketGuild guild, SocketUser user)
    {
        using var scope = services.CreateScope();

        var joinLeaveDb = scope.ServiceProvider.GetRequiredService<JoinLeaveDatabase>();

        var config = joinLeaveDb.JoinLeaveConfig.Find(user);

        if (config != null)
        {
            var channel = guild.GetTextChannel(config.LeaveChannelId);

            if (channel != null)
            {
                var message = config.LeaveMessage
                    .Replace("{SERVERNAME}", guild.Name)
                    .Replace("{USERNAME}", user.Username)
                    .Replace("{MENTION}", user.Mention);

                await channel.SendMessageAsync(message);
            }
        }
    }
}
