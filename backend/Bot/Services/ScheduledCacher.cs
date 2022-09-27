using Bot.Abstractions;
using Bot.Data;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Events;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Discord.WebSocket;
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
	private readonly DiscordSocketClient _client;

	private DateTime _nextCacheSchedule;

	public ScheduledCacher(DiscordRest discordRest, BotEventHandler eventHandler,
		IdentityManager identityManager, ILogger<ScheduledCacher> logger, CachedServices cachedServices,
		IServiceProvider serviceProvider, DiscordSocketClient client)
	{
		_discordRest = discordRest;
		_eventHandler = eventHandler;
		_identityManager = identityManager;
		_logger = logger;
		_cachedServices = cachedServices;
		_serviceProvider = serviceProvider;
		_client = client;
	}

	public void RegisterEvents()
	{
		_eventHandler.OnBotLaunched += StartCaching;
		_eventHandler.OnGuildRegistered += HandleGuildRegister;
		_client.UserLeft += HandleUserLeftCaching;
		_client.UserJoined += HandleUserJoinCaching;
	}

	private async Task HandleUserJoinCaching(SocketGuildUser user)
	{
		using var scope = _serviceProvider.CreateScope();

		var userRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();

		await userRepo.RemoveUserIfExists(user);
	}

	private async Task HandleUserLeftCaching(SocketGuild _, SocketUser user)
	{
		if(user.MutualGuilds.Count <= 0)
		{
			using var scope = _serviceProvider.CreateScope();

			var userRepo = scope.ServiceProvider.GetRequiredService<UserRepository>();

			await userRepo.AddUserIfDoesNotExist(user);
		}
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

		var handledUsers = await CacheAllKnownGuilds();

		_logger.LogInformation($"Cacher | Done with {handledUsers.Count} entries.");

		_eventHandler.InternalCachingDoneEvent.Invoke(handledUsers.Count, GetNextCacheSchedule());
	}

	public async Task<List<ulong>> CacheAllKnownGuilds()
	{
		var handledUsers = new List<ulong> ();

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

	public DateTime GetNextCacheSchedule()
	{
		return _nextCacheSchedule;
	}
}