using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Punishments.Data;
using Punishments.Models;

namespace Punishments.Controllers;

[Route("api/v1/guilds/{guildId}/dashboard")]
public class GuildDashboardController(IdentityManager identityManager, ModCaseCommentRepository modCaseCommentRepo,
    DiscordRest discordRest) : AuthenticatedController(identityManager, modCaseCommentRepo)
{
    private readonly DiscordRest _discordRest = discordRest;
    private readonly ModCaseCommentRepository _modCaseCommentRepo = modCaseCommentRepo;

    [HttpGet("latestcomments")]
    public async Task<IActionResult> LatestComments([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        List<ModCaseCommentExpanded> view = [];

        foreach (var comment in await _modCaseCommentRepo.GetLastCommentsByGuild(guildId))
        {
            view.Add(new ModCaseCommentExpandedTable(
                comment,
                await _discordRest.FetchUserInfo(comment.UserId, true),
                guildId,
                comment.ModCase.CaseId
            ));
        }

        return Ok(view);
    }
}
