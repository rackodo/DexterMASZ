using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.DTOs;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Bot.Controllers;

[Route("api/v1/settings")]
public class AppSettingsController : AuthenticatedController
{
	private readonly SettingsRepository _settingsRepository;

	public AppSettingsController(IdentityManager identityManager, SettingsRepository settingsRepository) :
		base(identityManager, settingsRepository)
	{
		_settingsRepository = settingsRepository;
	}

	[HttpGet]
	public async Task<IActionResult> GetAppSettings([FromRoute] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		return Ok(await _settingsRepository.GetAppSettings());
	}

	[HttpPut("embed")]
	public async Task<IActionResult> UpdateAppSettings([FromBody] EmbedAppSettingsForPutDto newSettings)
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		var toAdd = await _settingsRepository.GetAppSettings();

		toAdd.EmbedTitle = newSettings.EmbedTitle;
		toAdd.EmbedContent = newSettings.EmbedContent;

		await _settingsRepository.UpdateAppSetting(toAdd);

		return Ok(toAdd);
	}


	[HttpPut("infrastructure")]
	public async Task<IActionResult> UpdateAppSettings([FromBody] SettingsAppSettingsForPutDto newSettings)
	{
		var identity = await SetupAuthentication();

		await identity.RequireSiteAdmin();

		var toAdd = await _settingsRepository.GetAppSettings();

		toAdd.DefaultLanguage = newSettings.DefaultLanguage;
		toAdd.AuditLogWebhookUrl = newSettings.AuditLogWebhookURL ?? string.Empty;
		toAdd.PublicFileMode = newSettings.PublicFileMode;

		await _settingsRepository.UpdateAppSetting(toAdd);

		return Ok(toAdd);
	}
}
