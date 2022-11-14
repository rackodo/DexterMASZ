using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using GuildAudits.Data;
using GuildAudits.DTOs;
using GuildAudits.Enums;
using GuildAudits.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuildAudits.Controllers;

[Route("api/v1/guilds/{guildId}/auditlog")]
public class GuildAuditConfigController : AuthenticatedController
{
    private readonly GuildAuditConfigRepository _guildAuditRepo;

    public GuildAuditConfigController(IdentityManager identityManager, GuildAuditConfigRepository guildAuditRepo) :
        base(identityManager, guildAuditRepo) =>
        _guildAuditRepo = guildAuditRepo;

    [HttpPut]
    public async Task<IActionResult> SetItem([FromRoute] ulong guildId, [FromBody] GuildAuditConfigForPutDto dto)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        return Ok(await _guildAuditRepo.UpdateConfig(new GuildAuditConfig(dto, guildId)));
    }

    [HttpDelete("{type}")]
    public async Task<IActionResult> DeleteItem([FromRoute] ulong guildId, [FromRoute] GuildAuditLogEvent type)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        return Ok(await _guildAuditRepo.DeleteConfigForGuild(guildId, type));
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetItem([FromRoute] ulong guildId, [FromRoute] GuildAuditLogEvent type)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        return Ok(await _guildAuditRepo.GetConfigsByGuildAndType(guildId, type));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        return Ok(await _guildAuditRepo.GetConfigsByGuild(guildId));
    }
}