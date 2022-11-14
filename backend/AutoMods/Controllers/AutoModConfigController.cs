using AutoMods.Data;
using AutoMods.DTOs;
using AutoMods.Enums;
using AutoMods.Models;
using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMods.Controllers;

[Route("api/v1/guilds/{guildId}/automodconfig")]
public class AutoModConfigController : AuthenticatedController
{
    private readonly AutoModConfigRepository _autoModConfigRepository;

    public AutoModConfigController(IdentityManager identityManager, AutoModConfigRepository autoModConfigRepository) :
        base(identityManager, autoModConfigRepository) =>
        _autoModConfigRepository = autoModConfigRepository;

    [HttpPut]
    public async Task<IActionResult> SetItem([FromRoute] ulong guildId, [FromBody] AutoModConfigForPutDto dto)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var config = await _autoModConfigRepository.UpdateConfig(new AutoModConfig(dto, guildId));

        return Ok(config);
    }

    [HttpDelete("{type}")]
    public async Task<IActionResult> DeleteItem([FromRoute] ulong guildId, [FromRoute] AutoModType type)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var config = await _autoModConfigRepository.DeleteConfigForGuild(guildId, type);

        return Ok(config);
    }

    [HttpGet("{type}")]
    public async Task<IActionResult> GetItem([FromRoute] ulong guildId, [FromRoute] AutoModType type)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var config = await _autoModConfigRepository.GetConfigsByGuildAndType(guildId, type);

        return Ok(config);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var configs = await _autoModConfigRepository.GetConfigsByGuild(guildId);

        return Ok(configs);
    }
}