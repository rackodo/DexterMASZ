using System.ComponentModel.DataAnnotations;
using MASZ.AutoMods.Data;
using MASZ.AutoMods.Models;
using MASZ.AutoMods.Views;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.AutoMods.Controllers;

[Route("api/v1/guilds/{guildId}/autoMod")]
public class AutoModEventController : AuthenticatedController
{
	private readonly AutoModEventRepository _autoModEventRepository;
	private readonly GuildConfigRepository _guildConfigRepo;

	public AutoModEventController(IdentityManager identityManager, AutoModEventRepository autoModEventRepository,
		GuildConfigRepository guildConfigRepo) :
		base(identityManager, autoModEventRepository, guildConfigRepo)
	{
		_autoModEventRepository = autoModEventRepository;
		_guildConfigRepo = guildConfigRepo;
	}

	[HttpGet]
	public async Task<IActionResult> GetAllItems([FromRoute] ulong guildId,
		[FromQuery] [Range(0, int.MaxValue)] int startPage = 0)
	{
		var identity = await SetupAuthentication();

		await _guildConfigRepo.RequireGuildRegistered(guildId);

		ulong userOnly = 0;

		if (!await identity.HasPermission(DiscordPermission.Moderator, guildId))
			userOnly = identity.GetCurrentUser().Id;

		List<AutoModEvent> events;

		int eventsCount;

		if (userOnly == 0)
		{
			events = await _autoModEventRepository.GetPagination(guildId, startPage);
			eventsCount = await _autoModEventRepository.CountEventsByGuild(guildId);
		}
		else
		{
			events = await _autoModEventRepository.GetPaginationFilteredForUser(guildId, userOnly, startPage);
			eventsCount = await _autoModEventRepository.CountEventsByGuildAndUser(guildId, userOnly);
		}

		return Ok(new
		{
			events = events.Select(x => new AutoModEventView(x)),
			count = eventsCount
		});
	}
}