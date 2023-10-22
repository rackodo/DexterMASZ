using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using UserNotes.Data;
using UserNotes.DTOs;

namespace UserNotes.Controllers;

[Route("api/v1/guilds/{guildId}/usernote")]
public class UserNoteController(IdentityManager identityManager, UserNoteRepository userNoteRepo) : AuthenticatedController(identityManager, userNoteRepo)
{
    private readonly UserNoteRepository _userNoteRepo = userNoteRepo;

    [HttpGet]
    public async Task<IActionResult> GetUserNote([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userNotes = await _userNoteRepo.GetUserNotesByGuild(guildId);

        return Ok(userNotes);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserNote([FromRoute] ulong guildId, [FromRoute] ulong userId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        return Ok(await _userNoteRepo.GetUserNote(guildId, userId));
    }

    [HttpPut]
    public async Task<IActionResult> CreateUserNote([FromRoute] ulong guildId, [FromBody] UserNoteForUpdateDto userNote)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var createdUserNote =
            await _userNoteRepo.CreateOrUpdateUserNote(guildId, userNote.UserId, userNote.Description);

        return StatusCode(201, createdUserNote);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserNote([FromRoute] ulong guildId, [FromRoute] ulong userId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        await _userNoteRepo.DeleteUserNote(guildId, userId);

        return Ok();
    }
}
