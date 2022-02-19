using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Services;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MOTDs.Extensions;
using MOTDs.Models;

namespace MOTDs.Events;

public class MotdEventAnnouncer : Event
{
	private readonly MotdEventHandler _eventHandler;
	private readonly ILogger<MotdEventAnnouncer> _logger;
	private readonly IServiceProvider _serviceProvider;

	public MotdEventAnnouncer(MotdEventHandler eventHandler, ILogger<MotdEventAnnouncer> logger,
		IServiceProvider serviceProvider)
	{
		_eventHandler = eventHandler;
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnGuildMotdCreated += async (a, b) => await AnnounceMotd(a, b, RestAction.Created);

		_eventHandler.OnGuildMotdUpdated += async (a, b) => await AnnounceMotd(a, b, RestAction.Updated);
	}

	private async Task AnnounceMotd(GuildMotd motd, IUser actor, RestAction action)
	{
		using var scope = _serviceProvider.CreateScope();

		_logger.LogInformation($"Announcing motd {motd.GuildId} ({motd.Id}).");

		var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
			.GetGuildConfig(motd.GuildId);

		if (!string.IsNullOrEmpty(guildConfig.AdminWebhook))
		{
			_logger.LogInformation(
				$"Sending internal webhook for motd {motd.GuildId} ({motd.Id}) to {guildConfig.AdminWebhook}.");

			try
			{
				var embed = await motd.CreateMotdEmbed(actor, action, _serviceProvider);
				await DiscordRest.ExecuteWebhook(guildConfig.AdminWebhook, embed.Build());
			}
			catch (Exception e)
			{
				_logger.LogError(e,
					$"Error while announcing motd {motd.GuildId} ({motd.Id}) to {guildConfig.AdminWebhook}.");
			}
		}
	}
}