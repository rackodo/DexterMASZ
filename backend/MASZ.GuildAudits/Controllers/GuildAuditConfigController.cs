using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.GuildAudits.Data;
using MASZ.GuildAudits.DTOs;
using MASZ.GuildAudits.Enums;
using MASZ.GuildAudits.Models;
using MASZ.GuildAudits.Views;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.GuildAudits.Controllers;

[Route("api/v1/guilds/{guildId}/auditlog")]
public class GuildAuditConfigController : AuthenticatedController
{
	private readonly GuildAuditConfigRepository _guildAuditLogRepo;

	public GuildAuditConfigController(IdentityManager identityManager, GuildAuditConfigRepository guildAuditLogRepo) :
		base(identityManager, guildAuditLogRepo)
	{
		_guildAuditLogRepo = guildAuditLogRepo;
	}

	[HttpPut]
	public async Task<IActionResult> SetItem([FromRoute] ulong guildId, [FromBody] GuildAuditConfigForPutDto dto)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Admin, guildId);

		return Ok(new GuildAuditConfigView(await _guildAuditLogRepo.UpdateConfig(new GuildAuditConfig(dto, guildId))));
	}

	[HttpDelete("{type}")]
	public async Task<IActionResult> DeleteItem([FromRoute] ulong guildId, [FromRoute] GuildAuditEvent type)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Admin, guildId);

		return Ok(new GuildAuditConfigView(await _guildAuditLogRepo.DeleteConfigForGuild(guildId, type)));
	}

	[HttpGet("{type}")]
	public async Task<IActionResult> GetItem([FromRoute] ulong guildId, [FromRoute] GuildAuditEvent type)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Admin, guildId);

		return Ok(new GuildAuditConfigView(await _guildAuditLogRepo.GetConfigsByGuildAndType(guildId, type)));
	}

	[HttpGet]
	public async Task<IActionResult> GetAllItems([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Admin, guildId);

		return Ok((await _guildAuditLogRepo.GetConfigsByGuild(guildId))
			.Select(x => new GuildAuditConfigView(x)));
	}
}