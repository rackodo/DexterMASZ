using Bot.Abstractions;
using Bot.Services;
using Discord;
using Discord.WebSocket;
using JoinLeave.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            if (!config.Enabled)
                return;

            var channel = user.Guild.GetTextChannel(config.JoinChannelId);

            if (channel != null)
                await channel.SendMessageAsync(FormatMessage(config.JoinMessage, user.Guild, user));
        }
    }

    private async Task MemberLeft(SocketGuild guild, SocketUser user)
    {
        using var scope = services.CreateScope();

        var joinLeaveDb = scope.ServiceProvider.GetRequiredService<JoinLeaveDatabase>();

        var config = joinLeaveDb.JoinLeaveConfig.Find(user);

        if (config != null)
        {
            if (!config.Enabled)
                return;

            var channel = guild.GetTextChannel(config.LeaveChannelId);

            if (channel != null)
                await channel.SendMessageAsync(FormatMessage(config.LeaveMessage, guild, user));
        }
    }

    private static string FormatMessage(string message, IGuild guild, IUser user) => message
        .Replace("{SERVERNAME}", guild.Name)
        .Replace("{USERNAME}", user.Username)
        .Replace("{MENTION}", user.Mention);
}
