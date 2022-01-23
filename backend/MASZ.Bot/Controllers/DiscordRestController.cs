using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.DTOs;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Identities;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using MASZ.Bot.Views;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Bot.Controllers;

[Route("api/v1/discord")]
public class DiscordRestController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly GuildConfigRepository _guildConfigRepo;

	public DiscordRestController(DiscordRest discordRest, IdentityManager identityManager,
		GuildConfigRepository guildConfigRepo) :
		base(identityManager, guildConfigRepo)
	{
		_discordRest = discordRest;
		_guildConfigRepo = guildConfigRepo;
	}

	[HttpGet("users/@me")]
	public async Task<IActionResult> GetUser()
	{
		var identity = await SetupAuthentication();

		var currentUser = identity.GetCurrentUser();

		var currentUserGuilds = identity.GetCurrentUserGuilds();

		List<DiscordGuildView> userGuilds = new();
		List<DiscordGuildView> modGuilds = new();
		List<DiscordGuildView> adminGuilds = new();
		List<DiscordGuildView> bannedGuilds = new();

		if (identity is not DiscordOAuthIdentity)
			return Ok(new ApiUserDto(userGuilds, bannedGuilds, modGuilds, adminGuilds, currentUser,
				await identity.IsSiteAdmin()));

		var registeredGuilds = await _guildConfigRepo.GetAllGuildConfigs();

		foreach (var guild in registeredGuilds)
		{
			var userGuild = currentUserGuilds.FirstOrDefault(x => x.Id == guild.GuildId);

			if (userGuild != null)
			{
				var userGuildFetched = _discordRest.FetchGuildInfo(userGuild.Id, CacheBehavior.Default);

				if (userGuildFetched != null)
				{
					if (await identity.HasModRoleOrHigherOnGuild(guild.GuildId))
						if (await identity.HasAdminRoleOnGuild(guild.GuildId))
							adminGuilds.Add(new DiscordGuildView(userGuildFetched));
						else
							modGuilds.Add(new DiscordGuildView(userGuildFetched));
					else
						userGuilds.Add(new DiscordGuildView(userGuildFetched));
				}
			}
			else
			{
				try
				{
					_discordRest.GetFromCache<IBan>(CacheKey.GuildBan(guild.GuildId, currentUser.Id));
					bannedGuilds.Add(
						new DiscordGuildView(_discordRest.FetchGuildInfo(guild.GuildId, CacheBehavior.Default)));
				}
				catch (NotFoundInCacheException)
				{
				}
			}
		}

		return Ok(new ApiUserDto(userGuilds, bannedGuilds, modGuilds, adminGuilds, currentUser,
			await identity.IsSiteAdmin()));
	}

	[HttpGet("users/{userid}")]
	public async Task<IActionResult> GetSpecificUser([FromRoute] ulong userid)
	{
		var user = await _discordRest.FetchUserInfo(userid, CacheBehavior.OnlyCache);

		if (user != null)
			return Ok(new DiscordUserView(user));

		return NotFound();
	}

	[HttpGet("guilds/{guildId}")]
	public IActionResult GetSpecificGuild([FromRoute] ulong guildId)
	{
		var guild = _discordRest.FetchGuildInfo(guildId, CacheBehavior.Default);

		if (guild != null)
			return Ok(new DiscordGuildView(guild));

		return NotFound();
	}

	[HttpGet("guilds/{guildId}/channels")]
	public IActionResult GetAllGuildChannels([FromRoute] ulong guildId)
	{
		var channels = _discordRest.FetchGuildChannels(guildId, CacheBehavior.Default);

		if (channels != null)
			return Ok(channels.Select(x => new DiscordChannelView(x)));

		return NotFound();
	}

	[HttpGet("guilds/{guildId}/users")]
	public async Task<IActionResult> GetGuildUsers([FromRoute] ulong guildId)
	{
		await _guildConfigRepo.RequireGuildRegistered(guildId);

		var users = await _discordRest.FetchGuildUsers(guildId, CacheBehavior.OnlyCache);

		if (users != null)
			return Ok(users.Select(x => new DiscordUserView(x)));

		return NotFound();
	}

	[HttpGet("guilds")]
	public async Task<IActionResult> GetAllGuilds()
	{
		var identity = await SetupAuthentication();

		return Ok(identity.GetCurrentUserGuilds().Select(x => new DiscordGuildView(x)));
	}
}