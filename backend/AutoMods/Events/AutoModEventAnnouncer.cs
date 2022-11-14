using AutoMods.Enums;
using AutoMods.Extensions;
using AutoMods.Models;
using AutoMods.Translators;
using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoMods.Events;

public class AutoModEventAnnouncer : Event
{
    private readonly DiscordSocketClient _client;
    private readonly DiscordRest _discordRest;
    private readonly AutoModEventHandler _eventHandler;
    private readonly ILogger<AutoModEventAnnouncer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AutoModEventAnnouncer(DiscordRest discordRest, AutoModEventHandler eventHandler,
        ILogger<AutoModEventAnnouncer> logger, IServiceProvider serviceProvider, DiscordSocketClient client)
    {
        _discordRest = discordRest;
        _eventHandler = eventHandler;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _client = client;
    }

    public void RegisterEvents()
    {
        _eventHandler.OnAutoModEventRegistered += async (a, b, c, d, e) => await AnnounceAutoMod(a, b, c, d, e);

        _eventHandler.OnAutoModConfigCreated += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Created);

        _eventHandler.OnAutoModConfigUpdated += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Updated);

        _eventHandler.OnAutoModConfigDeleted += async (a, b) => await AnnounceAutoModConfig(a, b, RestAction.Deleted);
    }

    private async Task AnnounceAutoMod(AutoModEvent modEvent, AutoModConfig punishmentsConfig, GuildConfig guildConfig,
        ITextChannel channel, IUser author)
    {
        using var scope = _serviceProvider.CreateScope();

        var translator = scope.ServiceProvider.GetRequiredService<Translation>();

        translator.SetLanguage(guildConfig);

        _logger.LogInformation(
            $"Sending webhook for auto mod event {modEvent.GuildId}/{modEvent.Id} to {guildConfig.StaffLogs}.");

        try
        {
            var embed = await modEvent.CreateInternalAutoModEmbed(guildConfig, author, channel,
                scope.ServiceProvider, punishmentsConfig.PunishmentType);

            await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffLogs, embed);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} to {guildConfig.StaffLogs}.");
        }

        _logger.LogInformation(
            $"Sending dm notification for auto mod event {modEvent.GuildId}/{modEvent.Id} to {author.Id}.");

        try
        {
            var reason = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModType);
            var action = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModAction);
            await _discordRest.SendDmMessage(author.Id,
                translator.Get<AutoModNotificationTranslator>()
                    .NotificationAutoModDm(author, channel, reason, action));
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} in dm to {author.Id}.");
        }

        if (modEvent.AutoModAction is AutoModAction.ContentDeleted or AutoModAction.ContentDeletedAndCaseCreated &&
            punishmentsConfig.ChannelNotificationBehavior != AutoModChannelNotificationBehavior.NoNotification)
        {
            _logger.LogInformation(
                $"Sending channel notification to {modEvent.GuildId}/{modEvent.Id} {channel.GuildId}/{channel.Id}.");

            try
            {
                var reason = translator.Get<AutoModEnumTranslator>().Enum(modEvent.AutoModType);
                IMessage msg = await channel.SendMessageAsync(translator.Get<AutoModNotificationTranslator>()
                    .NotificationAutoModChannel(author, reason));

                if (punishmentsConfig.ChannelNotificationBehavior ==
                    AutoModChannelNotificationBehavior.SendNotificationAndDelete)
                {
                    async void Action()
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));

                        try
                        {
                            _logger.LogInformation(
                                $"Deleting channel auto mod event notification {channel.GuildId}/{channel.Id}/{msg.Id}.");
                            await msg.DeleteAsync();
                        }
                        catch (UnauthorizedException)
                        {
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e,
                                $"Error while deleting message {channel.GuildId}/{channel.Id}/{msg.Id} for auto mod event {modEvent.GuildId}/{modEvent.Id}.");
                        }
                    }

                    Task task = new(Action);

                    task.Start();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,
                    $"Error while announcing auto mod event {modEvent.GuildId}/{modEvent.Id} in channel {channel.Id}.");
            }
        }
    }

    private async Task AnnounceAutoModConfig(AutoModConfig config, IUser actor, RestAction action)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation($"Announcing auto mod config {config.GuildId}/{config.AutoModType} ({config.Id}).");

        var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
            .GetGuildConfig(config.GuildId);

        _logger.LogInformation(
            $"Sending webhook for config {config.GuildId}/{config.AutoModType} ({config.Id}) to {guildConfig.StaffLogs}.");

        var embed = await config.CreateAutoModConfigEmbed(actor, action, scope.ServiceProvider);

        await _client.SendEmbed(guildConfig.GuildId, guildConfig.StaffLogs, embed);
    }
}