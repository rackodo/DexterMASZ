using Bot.Abstractions;
using Bot.Data;
using Bot.Enums;
using Bot.Events;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Identities;
using Bot.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bot.Services;

public class IdentityManager : Event
{
	private readonly DiscordSocketClient _client;
	private readonly ILogger<IdentityManager> _logger;
	private readonly BotEventHandler _eventHandler;
	private readonly Dictionary<string, Identity> _identities;
	private readonly IServiceProvider _serviceProvider;

	public IdentityManager(BotEventHandler eventHandler, IServiceProvider serviceProvider,
		ILogger<IdentityManager> logger, DiscordSocketClient client)
	{
		_identities = new Dictionary<string, Identity>();

		_eventHandler = eventHandler;
		_serviceProvider = serviceProvider;

		_logger = logger;
		_client = client;
	}

	public void RegisterEvents()
	{
		_client.UserJoined += HandleUserJoined;
	}

	private Task HandleUserJoined(SocketGuildUser user)
	{
		foreach (var identity in GetCurrentIdentities().Where(identity => identity.GetCurrentUser().Id == user.Id))
			identity.AddGuildMembership(user);

		return Task.CompletedTask;
	}

	private async Task<Identity> RegisterNewIdentity(IUser user)
	{
		var key = $"/discord/cmd/{user.Id}";

		using var scope = _serviceProvider.CreateScope();

		var discordRest = _serviceProvider.GetRequiredService<DiscordRest>();

		var database = scope.ServiceProvider.GetRequiredService<BotDatabase>();

		var guildConfigs = await database.SelectAllGuildConfigs();
		List<UserGuild> guilds = new();

		foreach (var guildConfig in guildConfigs)
		{
			var gUser = await discordRest.FetchUserInfo(guildConfig.GuildId, user.Id, CacheBehavior.Default);

			guilds.Add(new UserGuild(gUser));
		}

		var identity = new DiscordCommandIdentity(user, guilds, _serviceProvider);
		_identities[key] = identity;

		_eventHandler.IdentityRegisteredEvent.Invoke(identity);

		return identity;
	}

	public async Task<Identity> GetIdentity(IUser user)
	{
		if (user is null)
			return null;

		var key = $"/discord/cmd/{user.Id}";

		if (!_identities.ContainsKey(key)) return await RegisterNewIdentity(user);

		var identity = _identities[key];

		if (identity.ValidUntil >= DateTime.UtcNow)
			return identity;

		_identities.Remove(key);

		return await RegisterNewIdentity(user);
	}

	public static string GetKeyByContext(HttpContext httpContext)
	{
		try
		{
			return httpContext.Request.Cookies["dexter_access_token"];
		}
		catch (KeyNotFoundException)
		{
			throw new UnauthorizedException();
		}
	}

	public async Task<Identity> RegisterNewIdentity(HttpContext httpContext)
	{
		var key = GetKeyByContext(httpContext);

		_logger.LogInformation("Registering new DiscordIdentity.");
		var token = await httpContext.GetTokenAsync("Cookies", "access_token");

		var rest = _serviceProvider.GetRequiredService<DiscordRest>();
		var user = await rest.FetchCurrentUserInfo(token, CacheBehavior.IgnoreButCacheOnError);
		var guilds = await rest.FetchGuildsOfCurrentUser(token, CacheBehavior.IgnoreButCacheOnError);

		var identity = new DiscordOAuthIdentity(token, _serviceProvider, user, guilds);

		if (identity is null)
			return null;

		_identities[key] = identity;

		_eventHandler.IdentityRegisteredEvent.Invoke(identity);

		return identity;
	}

	public void RemoveIdentity(HttpContext httpContext)
	{
		var key = GetKeyByContext(httpContext);

		var identity = _identities[key];

		_identities.Remove(key);

		_eventHandler.IdentityRemovedEvent.Invoke(identity);
	}

	public async Task<Identity> GetIdentity(HttpContext httpContext)
	{
		var key = GetKeyByContext(httpContext);

		if (string.IsNullOrEmpty(key))
			throw new UnauthorizedException();

		Identity identity;

		if (_identities.ContainsKey(key))
		{
			identity = _identities[key];

			if (identity is null)
				throw new InvalidIdentityException();

			if (identity.ValidUntil < DateTime.UtcNow)
				_identities.Remove(key);
			else
				return identity;
		}

		identity = await RegisterNewIdentity(httpContext);

		if (identity is null)
			throw new InvalidIdentityException();

		return identity;
	}

	public List<Identity> GetCurrentIdentities()
	{
		return _identities.Values.ToList();
	}

	public void ClearAllIdentities()
	{
		_identities.Clear();
	}

	public void ClearOldIdentities()
	{
		_logger.LogInformation("IdentityManager | Clearing old identities.");

		var count = 0;

		foreach (var key in _identities.Keys.Where(key => _identities[key].ValidUntil < DateTime.UtcNow))
		{
			count++;
			_identities.Remove(key);
		}

		_logger.LogInformation($"IdentityManager | Cleared {count} old identities.");
	}

	public void ClearIdentitiesOfType<T>()
	{
		_logger.LogInformation($"IdentityManager | Clearing {typeof(T).Name} identities.");

		var count = 0;

		foreach (var key in _identities.Keys.Where(key => _identities[key] is T))
		{
			count++;
			_identities.Remove(key);
		}

		_logger.LogInformation($"IdentityManager | Cleared {count} {typeof(T).Name} identities.");
	}
}