using Bot.Abstractions;
using Bot.Data;
using Bot.Extensions;
using Discord;
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
		using var scope = _serviceProvider.CreateScope();

		var settingsRepository = scope.ServiceProvider.GetRequiredService<SettingsRepository>();

		var config = await settingsRepository.GetAppSettings();

		QueueLog("======= STARTUP =======");
		QueueLog("`Dexter` started!");
		QueueLog("System time: " + DateTime.Now);
		QueueLog("System time (UTC): " + DateTime.UtcNow);
		QueueLog($"Language: `{config.DefaultLanguage}`");
		QueueLog($"URL: `{config.ServiceBaseUrl}`");
		QueueLog($"Domain: `{config.ServiceDomain}`");
		QueueLog($"Client ID: `{config.ClientId}`");

		QueueLog(config.CorsEnabled ? "CORS support: ⚠ `ENABLED`" : "CORS support: `DISABLED`");

		QueueLog("======= /STARTUP ========");

		await ExecuteWebhook();
	}

	public async Task StopAsync(CancellationToken _)
	{
		QueueLog("======= LOGOUT ========");

		await ExecuteWebhook();
	}

	public async void QueueLog(string message)
	{
		using var scope = _serviceProvider.CreateScope();

		var settingsRepository = scope.ServiceProvider.GetRequiredService<SettingsRepository>();

		var config = await settingsRepository.GetAppSettings();

		message = DateTime.UtcNow.ToDiscordTs() + " " + message[..Math.Min(message.Length, 1950)];

		if (string.IsNullOrEmpty(config.AuditLogWebhookUrl))
			return;

		if (_currentMessage.Length + message.Length <= 1998)
			_currentMessage.AppendLine(message);
		else
		{
			await ExecuteWebhook();
			_currentMessage.AppendLine(message);
		}
	}

	public async Task ExecuteWebhook()
	{
		if (_currentMessage.Length > 0)
		{
			_logger.LogInformation("Executing audit log webhook.");

			try
			{
				using var scope = _serviceProvider.CreateScope();

				var settingsRepository = scope.ServiceProvider.GetRequiredService<SettingsRepository>();

				var config = await settingsRepository.GetAppSettings();

				if (!string.IsNullOrEmpty(_currentMessage.ToString()))
					await DiscordRest.ExecuteWebhook(config.AuditLogWebhookUrl, null, _currentMessage.ToString(),
						AllowedMentions.None);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error executing audit log webhook. ");
			}

			_currentMessage.Clear();
		}
	}

	private async Task OnDisconnect(Exception _)
	{
		QueueLog("Bot **disconnected** from discord sockets.");
		await ExecuteWebhook();
	}

	private async Task OnBotReady()
	{
		QueueLog($"Bot **connected** to `{_client.Guilds.Count} guild(s)` with `{_client.Latency}ms` latency.");
		await ExecuteWebhook();
	}
}