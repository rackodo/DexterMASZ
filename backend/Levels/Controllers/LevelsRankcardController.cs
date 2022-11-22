using Bot.Abstractions;
using Bot.Exceptions;
using Bot.Services;
using Levels.Data;
using Levels.DTOs;
using Levels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Levels.Controllers;

[Route("api/v1/levels/rankcard")]
public class LevelsRankcardController : AuthenticatedController
{
    private readonly UserRankcardConfigRepository _levelsRankcardRepository;

    public LevelsRankcardController(IdentityManager identityManager, GuildLevelConfigRepository levelsConfigRepository,
        UserRankcardConfigRepository levelsRankcardRepository) :
        base(identityManager, levelsConfigRepository, levelsRankcardRepository) =>
        _levelsRankcardRepository = levelsRankcardRepository;

    [HttpGet("{userId}")]
    public IActionResult GetRankcardConfig([FromRoute] ulong userId)
    {
        var config = _levelsRankcardRepository.GetOrDefaultRankcard(userId);

        var result = new UserRankcardConfigDto(
            config.Id,
            config.XpColor,
            config.OffColor,
            config.LevelBgColor,
            config.TitleBgColor,
            config.Background,
            config.PfpRadiusFactor,
            config.TitleOffset,
            config.LevelOffset,
            config.PfpOffset,
            config.RankcardFlags);

        return Ok(result);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteRankcardConfig([FromRoute] ulong userId)
    {
        var identity = await SetupAuthentication();

        if (identity.GetCurrentUser().Id != userId && !await identity.IsSiteAdmin())
            throw new UnauthorizedException(
                "Insufficient permissions to delete another user's rankcard configuration.");

        await _levelsRankcardRepository.DeleteRankcard(userId);
        return Ok();
    }

    [HttpPost("")]
    public async Task<IActionResult> PutRankcardConfig([FromBody] UserRankcardConfigDto rankcardConfig)
    {
        var identity = await SetupAuthentication();

        if (identity.GetCurrentUser().Id != rankcardConfig.Id && !await identity.IsSiteAdmin())
            throw new UnauthorizedException(
                "Insufficient permissions to delete another user's rankcard configuration.");

        UserRankcardConfig newConfig = new()
        {
            Id = rankcardConfig.Id,
            XpColor = rankcardConfig.XpColor,
            OffColor = rankcardConfig.OffColor,
            LevelBgColor = rankcardConfig.LevelBgColor,
            TitleBgColor = rankcardConfig.TitleBgColor,
            Background = rankcardConfig.Background,
            PfpRadiusFactor = rankcardConfig.PfpRadiusFactor,
            RankcardFlags = rankcardConfig.RankcardFlags
        };

        if (rankcardConfig.PfpRadiusFactor < 0) return BadRequest("Pfp Radius Multiplier must be at least 0");
        if (rankcardConfig.PfpRadiusFactor > 2) return BadRequest("Pfp Radius Multiplier must be at most 2");

        newConfig.TitleOffset = rankcardConfig.TitleOffset;
        newConfig.LevelOffset = rankcardConfig.LevelOffset;
        newConfig.PfpOffset = rankcardConfig.PfpOffset;

        var existing = _levelsRankcardRepository.GetRankcard(newConfig.Id) is not null;
        Console.WriteLine($"Rankcard for ID {newConfig.Id} {(existing ? "exists" : "doesn't exist")}.");
        if (existing)
            await _levelsRankcardRepository.DeleteRankcard(newConfig.Id);

        await _levelsRankcardRepository.RegisterRankcard(newConfig);
        return existing ? Ok() : StatusCode(201);
    }
}
