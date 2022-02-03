using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace MASZ.Bot.Controllers;

[Route("api/v1/network")]
public class UserNetworkController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly GuildConfigRepository _guildConfigRepository;
	private readonly CachedServices _cachedServices;
	private readonly IServiceProvider _serviceProvider;

	public UserNetworkController(GuildConfigRepository guildConfigRepository, DiscordRest discordRest,
		IdentityManager identityManager, CachedServices cachedServices, IServiceProvider serviceProvider)
		: base(identityManager, guildConfigRepository)
	{
		_guildConfigRepository = guildConfigRepository;
		_discordRest = discordRest;
		_cachedServices = cachedServices;
		_serviceProvider = serviceProvider;
	}

	[HttpGet("user")]
	public async Task<IActionResult> GetUserNetwork([FromQuery][Required] ulong userId)
	{
		var identity = await SetupAuthentication();

		List<string> modGuilds = new();
		List<DiscordGuild> guildViews = new();

		var guildConfigs = await _guildConfigRepository.GetAllGuildConfigs();

		if (guildConfigs.Count == 0)
			throw new NoGuildsRegisteredException();

		foreach (var guildConfig in guildConfigs)
			if (await identity.HasPermission(DiscordPermission.Moderator, guildConfig.GuildId))
			{
				modGuilds.Add(guildConfig.GuildId.ToString());
				guildViews.Add(
					new DiscordGuild(_discordRest.FetchGuildInfo(guildConfig.GuildId, CacheBehavior.Default)));
			}

		if (modGuilds.Count == 0)
			return Unauthorized();

		DiscordUser searchedUser =
			new(await _discordRest.FetchUserInfo(userId, CacheBehavior.IgnoreButCacheOnError));

		dynamic data = new ExpandoObject();

		data.guilds = guildViews;
		data.user = searchedUser;

		foreach (var repo in
				 _cachedServices.GetInitializedAuthenticatedClasses<AddNetworks>(_serviceProvider, identity))
			await repo.AddNetworkData(data, modGuilds, userId);

		return Ok(data);
	}
}