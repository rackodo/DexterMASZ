using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Identities;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Dynamic;

namespace MASZ.Bot.Controllers;

[Route("api/v1/meta")]
public class AdminStatsController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly IdentityManager _identityManager;
	private readonly ILogger<AdminStatsController> _logger;
	private readonly ScheduledCacher _scheduler;
	private readonly CachedServices _cachedServices;
	private readonly IServiceProvider _serviceProvider;
	private readonly SettingsRepository _settingsRepository;

	public AdminStatsController(ILogger<AdminStatsController> logger, ScheduledCacher scheduler,
		SettingsRepository settingsRepository, DiscordRest discordRest, IdentityManager identityManager,
		IServiceProvider serviceProvider, CachedServices cachedServices) :
		base(identityManager, settingsRepository)
	{
		_logger = logger;
		_scheduler = scheduler;
		_settingsRepository = settingsRepository;
		_discordRest = discordRest;
		_serviceProvider = serviceProvider;
		_cachedServices = cachedServices;
		_identityManager = identityManager;
	}

	[HttpGet("adminStats")]
	public async Task<IActionResult> Status()
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		List<string> currentLogins = new();

		foreach (var login in _identityManager.GetCurrentIdentities().OfType<DiscordOAuthIdentity>())
			try
			{
				var user = login.GetCurrentUser();

				currentLogins.Add(user is null ? "Invalid user." : $"{user.Username}#{user.Discriminator}");
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error getting logged in user.");
				currentLogins.Add("Invalid user.");
			}

		var config = await _settingsRepository.GetAppSettings();

		dynamic adminStats = new ExpandoObject();

		adminStats.loginsInLast15Minutes = currentLogins;
		adminStats.defaultLanguage = config.DefaultLanguage;
		adminStats.nextCache = _scheduler.GetNextCacheSchedule();
		adminStats.cachedDataFromDiscord = _discordRest.GetCache().Keys;

		foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<AddAdminStats>(_serviceProvider,
					 identity))
			await repo.AddAdminStatistics(adminStats);

		return Ok(adminStats);
	}

	[HttpPost("cache")]
	public async Task<IActionResult> TriggerCache()
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		Task task = new(() =>
		{
			_identityManager.ClearAllIdentities();
			_scheduler.CacheAll();
		});

		task.Start();

		return Ok();
	}
}