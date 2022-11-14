using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using MOTDs.Data;
using MOTDs.DTOs;
using MOTDs.Views;

namespace MOTDs.Controllers;

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

        var creator = await _discordRest.FetchUserInfo(motd.UserId, false);

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