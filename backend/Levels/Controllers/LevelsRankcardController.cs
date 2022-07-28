using Bot.Abstractions;
using Bot.Exceptions;
using Bot.Services;
using Levels.Data;
using Levels.DTOs;
using Levels.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Controllers;

[Route("api/v1/levels/rankcard")]
public class LevelsRankcardController : AuthenticatedController
{
	private readonly GuildUserLevelRepository _levelsRepository;
	private readonly GuildLevelConfigRepository _levelsConfigRepository;
	private readonly UserRankcardConfigRepository _levelsRankcardRepository;

	public LevelsRankcardController(IdentityManager identityManager, GuildUserLevelRepository levelsRepository, GuildLevelConfigRepository levelsConfigRepository, UserRankcardConfigRepository levelsRankcardRepository) :
		base(identityManager, levelsRepository, levelsConfigRepository, levelsRankcardRepository)
	{
		_levelsRepository = levelsRepository;
		_levelsConfigRepository = levelsConfigRepository;
		_levelsRankcardRepository = levelsRankcardRepository;
	}

	[HttpGet("{userId}")]
	public IActionResult GetRankcardConfig([FromRoute] ulong userId)
	{
		UserRankcardConfig config = _levelsRankcardRepository.GetOrDefaultRankcard(userId);

		var result = new UserRankcardConfigDTO(
			id: config.Id,
			xpColor: config.XpColor,
			offColor: config.OffColor,
			levelBgColor: config.LevelBgColor,
			titleBgColor: config.TitleBgColor,
			background: config.Background,
			rankcardFlags: config.RankcardFlags);

		return Ok(result);
	}

	[HttpDelete("{userId}")]
	public async Task<IActionResult> DeleteRankcardConfig([FromRoute] ulong userId)
	{
		var identity = await SetupAuthentication();

		if (identity.GetCurrentUser().Id != userId && !await identity.IsSiteAdmin())
		{
			throw new UnauthorizedException("Insufficient permissions to delete another user's rankcard configuration.");
		}

		await _levelsRankcardRepository.DeleteRankcard(userId);
		return Ok();
	}

	[HttpPost("")]
	public async Task<IActionResult> PutRankcardConfig([FromBody] UserRankcardConfigDTO rankcardConfig)
	{
		var identity = await SetupAuthentication();

		if (identity.GetCurrentUser().Id != rankcardConfig.Id && !await identity.IsSiteAdmin())
		{
			throw new UnauthorizedException("Insufficient permissions to delete another user's rankcard configuration.");
		}

		UserRankcardConfig newConfig = new()
		{
			Id = rankcardConfig.Id,
			XpColor = rankcardConfig.XpColor,
			OffColor = rankcardConfig.OffColor,
			LevelBgColor = rankcardConfig.LevelBgColor,
			TitleBgColor = rankcardConfig.TitleBgColor,
			Background = rankcardConfig.Background,
			RankcardFlags = rankcardConfig.RankcardFlags
		};

		var existing = _levelsRankcardRepository.GetRankcard(newConfig.Id) is not null;
		Console.WriteLine($"Rankcard for ID {newConfig.Id} {(existing ? "exists" : "doesn't exist")}.");
		if (existing)
			await _levelsRankcardRepository.DeleteRankcard(newConfig.Id);

		await _levelsRankcardRepository.RegisterRankcard(newConfig);
		return existing ? Ok() : StatusCode(201);
	}
}
