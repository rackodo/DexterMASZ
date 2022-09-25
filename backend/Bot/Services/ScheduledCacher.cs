using Bot.Abstractions;
using Bot.Data;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Events;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Bot.Services;

public class ScheduledCacher : Event
{
	private const int CacheIntervalMinutes = 15;
	private readonly DiscordRest _discordRest;
	private readonly BotEventHandler _eventHandler;
	private readonly IdentityManager _identityManager;
	private readonly ILogger<ScheduledCacher> _logger;
	private readonly CachedServices _cachedServices;
	private readonly IServiceProvider _serviceProvider;

	private DateTime _nextCacheSchedule;

	public ScheduledCacher(ILogger<ScheduledCacher> logger, DiscordRest discordRest, IServiceProvider serviceProvider,
		IdentityManager identityManager, BotEventHandler eventHandler, CachedServices cachedServices)
	{
		_logger = logger;
		_discordRest = discordRest;
		_serviceProvider = serviceProvider;
		_identityManager = identityManager;
		_eventHandler = eventHandler;
		_cachedServices = cachedServices;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnBotLaunched += StartCaching;
		_eventHandler.OnGuildRegistered += HandleGuildRegister;
	}

	private async Task HandleGuildRegister(GuildConfig guildConfig, bool importExistingBans)
	{
		List<ulong> handledUsers = new();

		await CacheKnownGuild(guildConfig, handledUsers);

		_logger.LogInformation($"Cacher | Cached guild {guildConfig.GuildId} with {handledUsers.Count} entries.");

		using var scope = _serviceProvider.CreateScope();

		if (importExistingBans)
			foreach (var repo in _cachedServices.GetInitializedClasses<ImportGuildInfo>(scope.ServiceProvider))
				await repo.ImportGuildInfo(guildConfig);
	}

	public async Task StartCaching()
	{
		try
		{
			_logger.LogWarning("Starting schedule timers.");

			Timer eventTimer = new(TimeSpan.FromMinutes(CacheIntervalMinutes).TotalMilliseconds)
			{
				AutoReset = true,
				Enabled = true
			};

			eventTimer.Elapsed += async (_, _) => await LoopThroughCaches();

			await Task.Run(eventTimer.Start);

			_logger.LogWarning("Started schedule timers.");

			await LoopThroughCaches();
		}
		catch (Exception ex)
		{
			_logger.LogCritical(ex, "Something went wrong while starting the scheduler timer.");
		}
	}

	public async Task LoopThroughCaches()
	{
		try
		{
			_nextCacheSchedule = DateTime.UtcNow.AddMinutes(CacheIntervalMinutes);

			using (var scope = _serviceProvider.CreateScope())
			{
				foreach (var repo in _cachedServices.GetInitializedClasses<LoopCaches>(scope.ServiceProvider))
					await repo.LoopCaches();
			}

			CacheAll();
			_identityManager.ClearOldIdentities();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error in caching.");
		}
	}

	public async void CacheAll()
	{
		_logger.LogInformation("Cacher | Starting caching.");

		List<ulong> handledUsers = new();

		handledUsers = await CacheAllKnownGuilds(handledUsers);
		handledUsers = await CacheAllKnownUsers(handledUsers);

		_logger.LogInformation($"Cacher | Done with {handledUsers.Count} entries.");

		_eventHandler.InternalCachingDoneEvent.Invoke(handledUsers.Count, GetNextCacheSchedule());
	}

	public async Task<List<ulong>> CacheAllKnownGuilds(List<ulong> handledUsers)
	{
		_logger.LogInformation("Cacher | Cache all registered guilds.");

		using var scope = _serviceProvider.CreateScope();

		var database = scope.ServiceProvider.GetRequiredService<BotDatabase>();

		foreach (var guild in await database.SelectAllGuildConfigs())
		{
			try
			{
				handledUsers = await CacheKnownGuild(guild, handledUsers);
			}
			catch (GuildNotFoundException)
			{
				await database.DeleteSpecificGuildConfig(guild);
			}
		}

		return handledUsers;
	}
	
	public async Task<List<ulong>> CacheKnownGuild(GuildConfig guild, List<ulong> handledUsers)
	{
		var guildInst = _discordRest.FetchGuildInfo(guild.GuildId, CacheBehavior.IgnoreCache);

		if (guildInst == null)
		{
			_logger.LogError($"Cacher | Guild {guild.GuildId} does not exist!");
			throw new GuildNotFoundException();
		}

		var guildTag = $"{guildInst.Name} ({guildInst.Id})";

		_logger.LogInformation($"Cacher | Caching guild {guildTag}");

		_discordRest.FetchGuildChannels(guild.GuildId, CacheBehavior.IgnoreCache);

		_logger.LogInformation($"Cacher | Cache all bans of guild {guildTag}");

		var bans = await _discordRest.GetGuildBans(guild.GuildId, CacheBehavior.IgnoreCache);

		if (bans != null)
			handledUsers.AddRange(bans.Select(ban => ban.User.Id));

		_logger.LogInformation($"Cacher | Cache all users of guild {guildTag}");

		var users = await _discordRest.FetchGuildUsers(guild.GuildId, CacheBehavior.IgnoreCache);

		if (users != null)
			foreach (var item in users.Where(item => !handledUsers.Contains(item.Id)))
				handledUsers.Add(item.Id);

		return handledUsers;
	}

	public async Task<List<ulong>> CacheAllKnownUsers(List<ulong> handledUsers)
	{
		_logger.LogInformation("Cacher | Cache all known users.");

		using var scope = _serviceProvider.CreateScope();

		foreach (var repo in _cachedServices.GetInitializedClasses<CacheUsers>(scope.ServiceProvider))
			await repo.CacheKnownUsers(handledUsers);

		return handledUsers;
	}

	public DateTime GetNextCacheSchedule()
	{
		return _nextCacheSchedule;
	}
}