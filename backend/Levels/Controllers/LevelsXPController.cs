using Bot.Abstractions;
using Bot.Models;
using Bot.Services;
using Levels.Data;
using Levels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Levels.Controllers;

[Route("api/v1/levels")]
public class LevelsXpController : AuthenticatedController
{
    private const int MaxPageSize = 500;
    private readonly GuildLevelConfigRepository _levelsConfigRepository;
    private readonly GuildUserLevelRepository _levelsRepository;
    private readonly DiscordRest _rest;
    private readonly ILogger<LevelsXpController> _logger;

    public LevelsXpController(IdentityManager identityManager, GuildUserLevelRepository levelsRepository,
        GuildLevelConfigRepository levelsConfigRepository, DiscordRest rest, ILogger<LevelsXpController> logger) :
        base(identityManager, levelsRepository, levelsConfigRepository)
    {
        _levelsRepository = levelsRepository;
        _levelsConfigRepository = levelsConfigRepository;
        _rest = rest;
        _logger = logger;
    }

    [HttpGet("guilds/{guildId}/users/{userId}")]
    public async Task<IActionResult> GetGuildUserLevel([FromRoute] ulong guildId, [FromRoute] ulong userId)
    {
        var level = _levelsRepository.GetLevel(guildId, userId);
        if (level is null) return NotFound();

        var guildLevelConfig = await _levelsConfigRepository.GetOrCreateConfig(level.GuildId);

        var user = await _rest.FetchUserInfo(level.UserId, false);

        return Ok(new CalculatedGuildUserLevel(level, guildLevelConfig).ToDto(DiscordUser.GetDiscordUser(user)));
    }

    [HttpGet("guilds/{guildId}/users")]
    public async Task<IActionResult> GetLeaderboard([FromRoute] ulong guildId, [FromQuery] string order = "total",
        [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
    {
        _logger.LogInformation($"Received Leaderboard req for guild {guildId}, page {page} with size {pageSize}");
        if (pageSize < 1) pageSize = 1;
        else if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        Func<GuildUserLevel, long> func = order switch
        {
            "text" => l => l.TextXp,
            "voice" => l => l.VoiceXp,
            _ => l => l.TotalXp
        };

        var guildLevelConfig = await _levelsConfigRepository.GetOrCreateConfig(guildId);

        var allRecords = _levelsRepository.GetAllLevelsInGuild(guildId).OrderByDescending(func).AsQueryable();
        var selRecords = PagedList<GuildUserLevel>.ToPagedList(allRecords, page, pageSize);

        return Ok(selRecords.AsParallel().Select(async l =>
            {
                var user = await _rest.FetchUserInfo(l.UserId, true);
                return new CalculatedGuildUserLevel(l, guildLevelConfig).ToDto(DiscordUser.GetDiscordUser(user));
            }).Select(t => t.Result)
            .Where(r => r != null)
            .ToList());
    }
}
