using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using UserNotes.Data;
using UserNotes.Models;

namespace UserNotes.Controllers;

[Route("api/v1/guilds/{guildId}/usernoteview")]
public class UserNoteViewController(IdentityManager identityManager, UserNoteRepository userNoteRepo,
    DiscordRest discordRest) : AuthenticatedController(identityManager, userNoteRepo)
{
    private readonly DiscordRest _discordRest = discordRest;
    private readonly UserNoteRepository _userNoteRepo = userNoteRepo;

    [HttpGet]
    public async Task<IActionResult> GetGuildUserNoteView([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userNotes = await _userNoteRepo.GetUserNotesByGuild(guildId);
        List<UserNoteExpanded> userNoteViews = [];

        foreach (var userNote in userNotes)
        {
            userNoteViews.Add(new UserNoteExpanded(
                userNote,
                await _discordRest.FetchUserInfo(userNote.UserId, true),
                await _discordRest.FetchUserInfo(userNote.CreatorId, true)
            ));
        }

        return Ok(userNoteViews);
    }
}
