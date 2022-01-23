using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.MOTDs.Data;
using MASZ.MOTDs.DTOs;
using MASZ.MOTDs.Views;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.MOTDs.Controllers;

[Route("api/v1/guilds/{guildId}/motd")]
public class MotdController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly MotdRepository _guildMotdRepo;

	public MotdController(IdentityManager identityManager, MotdRepository guildMotdRepo, DiscordRest discordRest) :
		base(identityManager, guildMotdRepo)
	{
		_guildMotdRepo = guildMotdRepo;
		_discordRest = discordRest;
	}

	[HttpGet]
	public async Task<IActionResult> GetMotd([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var motd = await _guildMotdRepo.GetMotd(guildId);

		var creator = await _discordRest.FetchUserInfo(motd.UserId, CacheBehavior.Default);

		return Ok(new MotdExpandedView(motd, creator));
	}

	[HttpPut]
	public async Task<IActionResult> UpdateMotd([FromRoute] ulong guildId, [FromBody] MotdForCreateDto motd)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Admin, guildId);

		return Ok(await _guildMotdRepo.CreateOrUpdateMotd(guildId, motd.Message, motd.ShowMotd));
	}
}