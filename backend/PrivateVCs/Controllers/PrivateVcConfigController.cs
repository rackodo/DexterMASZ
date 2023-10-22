using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using PrivateVcs.Data;
using PrivateVcs.DTOs;
using PrivateVcs.Models;

namespace AutoMods.Controllers;

[Route("api/v1/guilds/{guildId}/privatevcconfig")]
public class PrivateVcConfigController(IdentityManager identityManager, PrivateVcConfigRepository privateVcConfigRepository) : AuthenticatedController(identityManager, privateVcConfigRepository)
{
    private readonly PrivateVcConfigRepository _privateVcConfigRepository = privateVcConfigRepository;

    [HttpPut]
    public async Task<IActionResult> SetConfig([FromRoute] ulong guildId, [FromBody] PrivateVcConfigDto dto)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var config = await _privateVcConfigRepository.PutPrivateVcConfig(new PrivateVcConfig(dto, guildId));

        return Ok(config);
    }

    [HttpGet]
    public async Task<IActionResult> GetConfig([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var config = await _privateVcConfigRepository.SelectPrivateVcConfig(guildId);

        return Ok(config);
    }
}
