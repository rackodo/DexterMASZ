using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Punishments.Data;

namespace Punishments.Controllers;

[Route("api/v1/guilds/{guildId}/bin")]
public class ModCaseBinController : AuthenticatedController
{
    private readonly ModCaseRepository _modCaseRepository;

    public ModCaseBinController(ModCaseRepository modCaseRepository, IdentityManager identityManager) :
        base(identityManager, modCaseRepository) =>
        _modCaseRepository = modCaseRepository;

    [HttpDelete("{caseId}/restore")]
    public async Task<IActionResult> RestoreModCase([FromRoute] ulong guildId, [FromRoute] int caseId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var modCase = await _modCaseRepository.RestoreCase(guildId, caseId);

        return Ok(modCase);
    }

    [HttpDelete("{caseId}/delete")]
    public async Task<IActionResult> DeleteModCase([FromRoute] ulong guildId, [FromRoute] int caseId)
    {
        var identity = await SetupAuthentication();

        await identity.RequireSiteAdmin();

        await _modCaseRepository.DeleteModCase(guildId, caseId, true);

        return Ok();
    }
}