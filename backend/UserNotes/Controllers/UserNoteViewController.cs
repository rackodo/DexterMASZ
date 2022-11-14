using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using UserNotes.Data;
using UserNotes.Models;

namespace UserNotes.Controllers;

[Route("api/v1/guilds/{guildId}/usernoteview")]
public class UserNoteViewController : AuthenticatedController
{
    private readonly DiscordRest _discordRest;
    private readonly UserNoteRepository _userNoteRepo;

    public UserNoteViewController(IdentityManager identityManager, UserNoteRepository userNoteRepo,
        DiscordRest discordRest) :
        base(identityManager, userNoteRepo)
    {
        _userNoteRepo = userNoteRepo;
        _discordRest = discordRest;
    }

    [HttpGet]
    public async Task<IActionResult> GetGuildUserNoteView([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userNotes = await _userNoteRepo.GetUserNotesByGuild(guildId);
        List<UserNoteExpanded> userNoteViews = new();

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