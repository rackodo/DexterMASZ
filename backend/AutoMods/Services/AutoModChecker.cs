using AutoMods.Data;
using AutoMods.Enums;
using AutoMods.MessageChecks;
using AutoMods.Models;
using Bot.Abstractions;
using Bot.Data;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoMods.Services;

public class AutoModChecker : IEvent
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<AutoModChecker> _logger;
    private readonly IServiceProvider _services;

    public AutoModChecker(DiscordSocketClient client, ILogger<AutoModChecker> logger, IServiceProvider services)
    {
        _client = client;
        _logger = logger;
        _services = services;
    }

    public void RegisterEvents()
    {
        _client.MessageReceived += MessageCreatedHandler;
        _client.MessageUpdated += MessageUpdatedHandler;
    }

    private async Task MessageCreatedHandler(SocketMessage message)
    {
        if (message.Channel is ITextChannel channel)
        {
            if (message.Type != MessageType.Default && message.Type != MessageType.Reply)
                return;

            if (message.Author.IsBot)
                return;

            if (channel.Guild == null)
                return;

            await HandleAutoMod(message);
        }
    }

    private async Task MessageUpdatedHandler(Cacheable<IMessage, ulong> oldMsg, SocketMessage newMsg,
        ISocketMessageChannel channel)
    {
        if (channel is ITextChannel txtChannel)
        {
            if (newMsg.Type != MessageType.Default && newMsg.Type != MessageType.Reply)
                return;

            if (newMsg.Author.IsBot)
                return;

            if (txtChannel.Guild == null)
                return;

            await HandleAutoMod(newMsg, true);
        }
    }

    public async Task HandleAutoMod(IMessage message, bool onEdit = false)
    {
        if (message.Type != MessageType.Default && message.Type != MessageType.Reply)
            return;

        if (message.Author.IsBot)
            return;

        if ((message.Channel as ITextChannel)?.Guild == null)
            return;

        using var scope = _services.CreateScope();

        if (!onEdit)
            if (await CheckAutoMod(
                    AutoModType.TooManyMessages,
                    message,
                    SpamCheck.Check,
                    scope
                ))
                return;

        if (await CheckAutoMod(
                AutoModType.InvitePosted,
                message,
                InviteChecker.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.TooManyEmotes,
                message,
                EmoteCheck.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.TooManyMentions,
                message,
                MentionCheck.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.TooManyAttachments,
                message,
                AttachmentCheck.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.TooManyEmbeds,
                message,
                EmbedCheck.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.CustomWordFilter,
                message,
                CustomWordCheck.Check,
                scope
            )) return;

        if (await CheckAutoMod(
                AutoModType.TooManyDuplicatedCharacters,
                message,
                DuplicatedCharacterCheck.Check,
                scope
            )) return;

        await CheckAutoMod(
            AutoModType.TooManyLinks,
            message,
            LinkCheck.Check,
            scope
        );
    }

    private async Task<bool> CheckAutoMod(AutoModType autoModType, IMessage message,
        Func<IMessage, AutoModConfig, DiscordSocketClient, Task<bool>> predicate, IServiceScope scope)
    {
        var guild = ((ITextChannel)message.Channel).Guild;

        var autoModConfig = (await scope.ServiceProvider.GetRequiredService<AutoModConfigRepository>()
                .GetConfigsByGuild(guild.Id))
            .FirstOrDefault(x => x.AutoModType == autoModType);

        if (autoModConfig == null) return false;

        if (!await predicate(message, autoModConfig, _client)) return false;

        if (await IsProtectedByFilter(message, autoModConfig, scope)) return false;

        var channel = (ITextChannel)message.Channel;

        _logger.LogInformation(
            $"U: {message.Author.Id} | C: {channel.Id} | G: {channel.Guild.Id} triggered {autoModConfig.AutoModType}.");

        await ExecutePunishment(message, autoModConfig, scope);

        if (autoModConfig.AutoModType != AutoModType.TooManyAutomods)
            await CheckAutoMod(AutoModType.TooManyAutomods, message, CheckMultipleEvents, scope);

        return true;
    }

    private async Task<bool> CheckAutoMod(AutoModType autoModType, IMessage message,
        Func<IMessage, AutoModConfig, DiscordSocketClient, bool> predicate, IServiceScope scope)
    {
        var guild = ((ITextChannel)message.Channel).Guild;

        var autoModConfig = (await scope.ServiceProvider.GetRequiredService<AutoModConfigRepository>()
                .GetConfigsByGuild(guild.Id))
            .FirstOrDefault(x => x.AutoModType == autoModType);

        if (autoModConfig == null) return false;

        if (!predicate(message, autoModConfig, _client)) return false;

        if (await IsProtectedByFilter(message, autoModConfig, scope)) return false;

        var channel = (ITextChannel)message.Channel;

        _logger.LogInformation(
            $"U: {message.Author.Id} | C: {channel.Id} | G: {channel.Guild.Id} triggered {autoModConfig.AutoModType}.");

        await ExecutePunishment(message, autoModConfig, scope);

        if (autoModConfig.AutoModType != AutoModType.TooManyAutomods)
            await CheckAutoMod(AutoModType.TooManyAutomods, message, CheckMultipleEvents, scope);

        return true;
    }

    private async Task CheckAutoMod(AutoModType autoModType, IMessage message,
        Func<IMessage, AutoModConfig, IServiceScope, Task<bool>> predicate, IServiceScope scope)
    {
        var guild = ((ITextChannel)message.Channel).Guild;

        var autoModConfig = (await scope.ServiceProvider.GetRequiredService<AutoModConfigRepository>()
                .GetConfigsByGuild(guild.Id))
            .FirstOrDefault(x => x.AutoModType == autoModType);

        if (autoModConfig == null) return;

        if (!await predicate(message, autoModConfig, scope)) return;

        if (await IsProtectedByFilter(message, autoModConfig, scope)) return;

        var channel = (ITextChannel)message.Channel;

        _logger.LogInformation(
            $"U: {message.Author.Id} | C: {channel.Id} | G: {channel.Guild.Id} triggered {autoModConfig.AutoModType}.");

        await ExecutePunishment(message, autoModConfig, scope);

        if (autoModConfig.AutoModType != AutoModType.TooManyAutomods)
            await CheckAutoMod(AutoModType.TooManyAutomods, message, CheckMultipleEvents, scope);
    }

    private static async Task<bool> IsProtectedByFilter(IMessage message, AutoModConfig autoModConfig,
        IServiceScope scope)
    {
        var config = await scope.ServiceProvider.GetRequiredService<SettingsRepository>().GetAppSettings();

        if (config.SiteAdmins.Contains(message.Author.Id))
            return true;

        var guild = ((ITextChannel)message.Channel).Guild;

        var user = await guild.GetUserAsync(message.Author.Id);

        if (user == null)
            return false;

        if (user.Guild.OwnerId == user.Id)
            return true;

        var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
            .GetGuildConfig(guild.Id);

        return user.RoleIds.Any(x =>
                   guildConfig.ModRoles.Contains(x) ||
                   guildConfig.AdminRoles.Contains(x) ||
                   autoModConfig.IgnoreRoles.Contains(x)
               ) ||
               (message.Channel is ITextChannel { CategoryId: { } } textChannel
                   ? autoModConfig.IgnoreChannels.Contains(message.Channel.Id) ||
                     autoModConfig.IgnoreChannels.Contains(textChannel.CategoryId.Value)
                   : autoModConfig.IgnoreChannels.Contains(((ITextChannel)message.Channel)!.Id));
    }

    private static async Task<bool> CheckMultipleEvents(IMessage message, AutoModConfig config, IServiceScope scope)
    {
        if (config.Limit == null)
            return false;

        if (config.TimeLimitMinutes == null)
            return false;

        var existing = await scope.ServiceProvider.GetRequiredService<AutoModEventRepository>()
            .GetAllEventsForUserSinceMinutes(message.Author.Id, config.TimeLimitMinutes.Value);

        return existing.Where(x => x.AutoModType != AutoModType.TooManyAutomods).Count() > config.Limit.Value;
    }

    private async Task ExecutePunishment(IMessage message, AutoModConfig autoModConfig, IServiceScope scope)
    {
        AutoModEvent modEvent = new()
        {
            GuildId = ((ITextChannel)message.Channel).Guild.Id,
            AutoModType = autoModConfig.AutoModType,
            AutoModAction = autoModConfig.AutoModAction,
            UserId = message.Author.Id,
            MessageId = message.Id,
            MessageContent = message.Content
        };

        await scope.ServiceProvider.GetRequiredService<AutoModEventRepository>()
            .RegisterEvent(modEvent, (ITextChannel)message.Channel, message.Author);

        if (modEvent.AutoModAction is AutoModAction.ContentDeleted or AutoModAction.ContentDeletedAndCaseCreated)
            try
            {
                RequestOptions requestOptions = new()
                {
                    RetryMode = RetryMode.RetryRatelimit
                };
                await message.DeleteAsync(requestOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting message {message.Id}.");
            }
    }
}
