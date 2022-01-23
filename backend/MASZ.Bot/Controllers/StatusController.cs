using System.Globalization;
using MASZ.Bot.Data;
using MASZ.Bot.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Bot.Controllers;

[ApiController]
[Route("api/v1")]
public class StatusController : ControllerBase
{
	private readonly SettingsRepository _settingsRepository;

	public StatusController(SettingsRepository settingsRepository)
	{
		_settingsRepository = settingsRepository;
	}

	[HttpGet("status")]
	[HttpGet("health")]
	[HttpGet("healthcheck")]
	[HttpGet("ping")]
	public async Task<IActionResult> Status()
	{
		var config = await _settingsRepository.GetAppSettings();

		if (!HttpContext.Request.Headers.ContainsKey("Accept")) return Ok("OK");

		if (HttpContext.Request.Headers["Accept"].ToString().Search("application/json"))
			return Ok(new
			{
				status = "OK",
				lang = config.DefaultLanguage,
				name = config.ServiceHostName,
				server_time = DateTime.Now.ToString(CultureInfo.InvariantCulture),
				server_time_utc = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
			});

		return Ok("OK");
	}
}