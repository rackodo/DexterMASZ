using Bot.Abstractions;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using UserMaps.Data;
using UserMaps.DTOs;

namespace UserMaps.Controllers;

[Route("api/v1/guilds/{guildId}/usermap")]
public class UserMapController(IdentityManager identityManager, UserMapRepository userMapRepo) : AuthenticatedController(identityManager, userMapRepo)
{
    private readonly UserMapRepository _userMapRepo = userMapRepo;

    [HttpGet]
    public async Task<IActionResult> GetUserMap([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userMaps = await _userMapRepo.GetUserMapsByGuild(guildId);

        return Ok(userMaps);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserMaps([FromRoute] ulong guildId, [FromRoute] ulong userId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userMaps = await _userMapRepo.GetUserMapsByGuildAndUser(guildId, userId);

        return Ok(userMaps);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserMap([FromRoute] ulong guildId, [FromBody] UserMapForCreateDto userMapDto)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        try
        {
            await _userMapRepo.GetUserMap(guildId, userMapDto.UserA, userMapDto.UserB);
            throw new ResourceAlreadyExists();
        }
        catch (ResourceNotFoundException)
        {
        }

        var userMap =
            await _userMapRepo.CreateOrUpdateUserMap(guildId, userMapDto.UserA, userMapDto.UserB, userMapDto.Reason);

        return StatusCode(201, userMap);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserMap([FromRoute] ulong guildId, [FromRoute] int id,
        [FromBody] UserMapForUpdateDto userMapDto)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userMap = await _userMapRepo.GetUserMap(id);

        if (userMap.GuildId != guildId)
            throw new ResourceNotFoundException();

        var result = await _userMapRepo.CreateOrUpdateUserMap(guildId, userMap.UserA, userMap.UserB, userMapDto.Reason);

        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserMap([FromRoute] ulong guildId, [FromRoute] int id)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Moderator, guildId);

        var userMap = await _userMapRepo.GetUserMap(id);

        if (userMap.GuildId != guildId)
            throw new ResourceNotFoundException();

        await _userMapRepo.DeleteUserMap(id);

        return Ok();
    }
}
