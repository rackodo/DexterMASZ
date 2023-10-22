using Bot.Abstractions;
using Bot.Data;
using Bot.DTOs;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Controllers;

[Route("api/v1/settings")]
public class AppSettingsController(IdentityManager identityManager, SettingsRepository settingsRepository) : AuthenticatedController(identityManager, settingsRepository)
{
    private readonly SettingsRepository _settingsRepository = settingsRepository;

    [HttpGet]
    public async Task<IActionResult> GetAppSettings()
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
        toAdd.AuditLogWebhookUrl = newSettings.AuditLogWebhookUrl ?? string.Empty;

        await _settingsRepository.UpdateAppSetting(toAdd);

        return Ok(toAdd);
    }
}
