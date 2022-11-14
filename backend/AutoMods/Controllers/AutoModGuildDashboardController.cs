using AutoMods.Data;
using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoMods.Controllers;

[Route("api/v1/guilds/{guildId}/dashboard")]
public class AutoModGuildDashboardController : AuthenticatedController
{
    private readonly AutoModEventRepository _autoModRepo;

    public AutoModGuildDashboardController(IdentityManager identityManager, AutoModEventRepository autoModRepo) :
        base(identityManager, autoModRepo) =>
        _autoModRepo = autoModRepo;

    [HttpGet("autoModChart")]
    public async Task<IActionResult> GetAutoModSplitChart([FromRoute] ulong guildId, [FromQuery] long? since = null)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var sinceTime = DateTime.UtcNow.AddYears(-1);

        if (since != null)
            sinceTime = DateTime.UnixEpoch.AddSeconds(since.Value);

        return Ok(await _autoModRepo.GetCountsByType(guildId, sinceTime));
    }
}