using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Abstractions;
using Bot.Services;
using Levels.Data;
using Levels.DTOs;
using Levels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Levels.Controllers;

[Route("api/v1/levels")]
public class LevelsXPController : AuthenticatedController
{
	private readonly GuildUserLevelRepository _levelsRepository;
	private readonly GuildLevelConfigRepository _levelsConfigRepository;

	public LevelsXPController(IdentityManager identityManager, GuildUserLevelRepository levelsRepository, GuildLevelConfigRepository levelsConfigRepository) :
		base(identityManager, levelsRepository, levelsConfigRepository)
	{
		_levelsRepository = levelsRepository;
		_levelsConfigRepository = levelsConfigRepository;
	}

	[HttpGet("guilds/{guildId}/users/{userId}")]
	public async Task<IActionResult> GetGuildUserLevel([FromRoute] ulong guildId, [FromRoute] ulong userId)
	{
		var level = _levelsRepository.GetLevel(guildId, userId);
		if (level is null) return NotFound();

		return Ok(await levelToDTO(level));
	}

	private async Task<GuildUserLevelDTO> levelToDTO(GuildUserLevel level)
	{
		GuildLevelConfig guildLevelConfig = await _levelsConfigRepository.GetOrCreateConfig(level.GuildId);
		return levelToDTO(level, guildLevelConfig);
	}

	private GuildUserLevelDTO levelToDTO(GuildUserLevel level, GuildLevelConfig config)
    {
		var calc = new CalculatedGuildUserLevel(level, config);
		return calc.ToDTO();
	}

	const int MAX_PAGE_SIZE = 500;
	[HttpGet("guilds/{guildId}/users")]
	public async Task<IActionResult> GetLeaderboard([FromRoute] ulong guildId, [FromQuery] string order = "total", [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
    {
		Console.WriteLine($"Received Leaderboard req for guild {guildId}, page {page} with size {pageSize}");
		if (pageSize < 1) pageSize = 1;
		else if (pageSize > MAX_PAGE_SIZE) pageSize = MAX_PAGE_SIZE;

		Func<GuildUserLevel, long> func = order switch
		{
			"text" => (l) => l.TextXp,
			"voice" => (l) => l.VoiceXp,
			_ => (l) => l.TotalXP
		};

		GuildLevelConfig guildLevelConfig = await _levelsConfigRepository.GetOrCreateConfig(guildId);

		var allRecords = _levelsRepository.GetAllLevelsInGuild(guildId).OrderByDescending(func).AsQueryable();
		var selRecords = PagedList<GuildUserLevel>.ToPagedList(allRecords, page, pageSize);

		return Ok(selRecords.Select(l => levelToDTO(l, guildLevelConfig)));
    }
}
