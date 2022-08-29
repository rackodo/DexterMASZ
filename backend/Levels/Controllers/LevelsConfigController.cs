using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Services;
using Levels.Data;
using Levels.Models;
using Microsoft.AspNetCore.Mvc;

namespace Levels.Controllers;

[Route("api/v1/guilds/{guildId}/levels")]
public class LevelsConfigController : AuthenticatedController
{
	private readonly GuildConfigRepository _guildConfigRepository;
	private readonly GuildLevelConfigRepository _levelsConfigRepository;

	public LevelsConfigController(IdentityManager identityManager, GuildLevelConfigRepository levelsConfigRepository, GuildConfigRepository guildConfigRepository) :
		base(identityManager, levelsConfigRepository)
	{
		_levelsConfigRepository = levelsConfigRepository;
		_guildConfigRepository = guildConfigRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetConfig([FromRoute] ulong guildId)
	{
		try
		{
			await _guildConfigRepository.GetGuildConfig(guildId);
		}
		catch (UnregisteredGuildException e)
		{
			return NotFound(e);
		}

		var config = await _levelsConfigRepository.GetOrCreateConfig(guildId);
		return Ok(config);
	}

	[HttpPut]
	public async Task<IActionResult> PutConfig([FromRoute] ulong guildId, [FromBody] GuildLevelConfig config)
	{
		var identity = await SetupAuthentication();

		if (!await identity.HasPermission(Discord.GuildPermission.Administrator, guildId) && !await identity.IsSiteAdmin())
			return Forbid();

		try
		{
			await _guildConfigRepository.GetGuildConfig(guildId);
		}
		catch (UnregisteredGuildException e)
		{
			return NotFound(e);
		}

		if (config.Id != guildId)
			return BadRequest("The IDs supplied in the request route and in the object in the request body do not match.");

		var validationErrors = ValidateConfig(config);
		if (validationErrors is not null)
		{
			return validationErrors;
		}

		var existing = await _levelsConfigRepository.GetOrCreateConfig(guildId);

		existing.Coefficients = config.Coefficients;
		existing.XpInterval = config.XpInterval;
		existing.MinimumTextXpGiven = config.MinimumTextXpGiven;
		existing.MaximumTextXpGiven = config.MaximumTextXpGiven;
		existing.MinimumVoiceXpGiven = config.MinimumVoiceXpGiven;
		existing.MaximumVoiceXpGiven = config.MaximumVoiceXpGiven;
		existing.VoiceXpRequiredMembers = config.VoiceXpRequiredMembers;
		existing.VoiceXpCountMutedMembers = config.VoiceXpCountMutedMembers;

		existing.HandleRoles = config.HandleRoles;
		existing.NicknameDisabledRole = config.NicknameDisabledRole;
		existing.NicknameDisabledReplacement = config.NicknameDisabledReplacement;
		existing.Levels = config.Levels;
		existing.LevelUpMessageOverrides = config.LevelUpMessageOverrides;
		existing.DisabledXpChannels = config.DisabledXpChannels;

		existing.LevelUpTemplate = config.LevelUpTemplate;
		existing.SendTextLevelUps = config.SendTextLevelUps;
		existing.SendVoiceLevelUps = config.SendVoiceLevelUps;
		existing.TextLevelUpChannel = config.TextLevelUpChannel;
		existing.VoiceLevelUpChannel = config.VoiceLevelUpChannel;

		await _levelsConfigRepository.UpdateConfig(config);
		return Ok();
	}

	private IActionResult? ValidateConfig(GuildLevelConfig config)
	{
		if (config.Coefficients.Length < 2 || config.Coefficients.Length > 10)
			return BadRequest("Leveling coefficients must have between 2 and 10 elements!");

		config.Coefficients[0] = 0;
		for (var i = 1; i < config.Coefficients.Length; i++)
		{
			var n = config.Coefficients[i];
			if (n <= 0) return BadRequest("Found non-positive coefficients in configuration.");
		}

		if (config.XpInterval < 10)
			return BadRequest("Experience Interval cannot be lower than 10 seconds");

		if (config.MaximumTextXpGiven < config.MinimumTextXpGiven)
			return BadRequest("Invalid range for Text Experience, minimum exceeds maximum.");
		if (config.MaximumVoiceXpGiven < config.MinimumVoiceXpGiven)
			return BadRequest("Invalid range for Voice Experience, minimum exceeds maximum.");

		if (config.NicknameDisabledReplacement != default)
		{
			var found = false;
			foreach (var roleIds in config.Levels.Values)
			{
				if (roleIds.Contains(config.NicknameDisabledReplacement))
				{
					found = true;
					break;
				}
			}
			if (!found) return BadRequest("Nickname Disabled Replacement must be a level role.");
		}

		if (string.IsNullOrWhiteSpace(config.LevelUpTemplate))
		{
			return BadRequest("Level Up Template must not be empty.");
		}

		if (config.LevelUpTemplate.Length > 200)
		{
			return BadRequest("The Length of Level Up Template may not exceed 200 characters.");
		}

		return null;
	}
}
