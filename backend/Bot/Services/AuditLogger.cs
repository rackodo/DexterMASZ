using Bot.Abstractions;
using Bot.Data;
using Bot.Extensions;
using Bot.Models;
using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Bot.Services;

public class AuditLogger : IHostedService, Event
{
	private readonly DiscordSocketClient _client;
	private readonly StringBuilder _currentMessage;
	private readonly ILogger<AuditLogger> _logger;
	private readonly IServiceProvider _serviceProvider;

	public AuditLogger(ILogger<AuditLogger> logger, IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_client = client;
		_currentMessage = new StringBuilder();
	}

	public void RegisterEvents()
	{
		_client.Connected += OnBotReady;
		_client.Disconnected += OnDisconnect;
	}

	public async Task StartAsync(CancellationToken _)
	{
		var config = await GetConfig();

		await QueueLog("======= STARTUP =======");
		await QueueLog("`Dexter` started!");
		await QueueLog("System time: " + DateTime.Now);
		await QueueLog("System time (UTC): " + DateTime.UtcNow);
		await QueueLog($"Language: `{config.DefaultLanguage}`");
		await QueueLog($"URL: `{config.GetServiceUrl()}`");
		await QueueLog($"Domain: `{config.ServiceDomain}`");
		await QueueLog($"Client ID: `{config.ClientId}`");

		await QueueLog(config.CorsEnabled ? "CORS support: ⚠ `ENABLED`" : "CORS support: `DISABLED`");

		await QueueLog("======= /STARTUP ========", true);
	}

	public async Task StopAsync(CancellationToken _)
	{
		await QueueLog("======= LOGOUT ========", true);
	}

	public async Task QueueLog(string message, bool shouldExecute = false)
	{
		message = DateTime.UtcNow.ToDiscordTs() + " " + message[..Math.Min(message.Length, 1950)];

		if (_currentMessage.Length + message.Length <= 1998)
		{
			_currentMessage.AppendLine(message);

			if (shouldExecute)
				await ExecuteWebhook();
		}
		else
		{
			await ExecuteWebhook();
			_currentMessage.AppendLine(message);
		}
	}

	public async Task<AppSettings> GetConfig()
	{
		using var scope = _serviceProvider.CreateScope();

		var settingsRepository = scope.ServiceProvider.GetRequiredService<SettingsRepository>();

		return await settingsRepository.GetAppSettings();
	}

	private async Task ExecuteWebhook()
	{
		var config = await GetConfig();

		if (string.IsNullOrEmpty(config.AuditLogWebhookUrl))
			return;

		StringBuilder msg;

		lock (_currentMessage)
		{
			if (_currentMessage.Length <= 0)
				return;

			msg = new StringBuilder(_currentMessage.Length);
			msg.Append(_currentMessage);
			_currentMessage.Clear();
		}

		_logger.LogInformation("Executing audit log webhook.");

		try
		{
			if (!string.IsNullOrEmpty(msg.ToString()))
				await new DiscordWebhookClient(config.AuditLogWebhookUrl).SendMessageAsync(msg.ToString(),
					allowedMentions: AllowedMentions.None);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error executing audit log webhook. ");
		}
	}

	private async Task OnDisconnect(Exception _)
	{
		await QueueLog("Bot **disconnected** from discord sockets.", true);
	}

	private async Task OnBotReady()
	{
		await QueueLog($"Bot **connected** to `{_client.Guilds.Count} guild(s)` with `{_client.Latency}ms` latency.", true);
	}
}