using MASZ.Bot.Data;
using MASZ.Bot.Services;
using MASZ.Bot.Views;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace MASZ.Bot.Controllers;

[ApiController]
[Route("api/v1/meta")]
public class MetaController : ControllerBase
{
	private readonly DiscordRest _discordRest;
	private readonly SettingsRepository _settingsRepository;

	public MetaController(DiscordRest discordRest, SettingsRepository settingsRepository)
	{
		_discordRest = discordRest;
		_settingsRepository = settingsRepository;
	}

	[HttpGet("user")]
	public IActionResult GetBotUser()
	{
		return Ok(new DiscordUserView(_discordRest.GetCurrentBotInfo()));
	}

	[HttpGet("embed")]
	public async Task<IActionResult> GetOEmbedInfo()
	{
		var appSettings = await _settingsRepository.GetAppSettings();

		return new ContentResult()
		{
			Content = appSettings.GetEmbedData(appSettings.ServiceBaseUrl),
			ContentType = "text/html"
		};
	}

	[HttpGet("application")]
	public async Task<IActionResult> GetApplication()
	{
		return Ok(new DiscordApplicationView(await _discordRest.GetCurrentApplicationInfo()));
	}

	[HttpGet("versions")]
	public async Task<IActionResult> GetReleases()
	{
		var restClient = new RestClient("https://maszindex.zaanposni.com/");
		var request = new RestRequest("/api/v1/versions");
		request.AddQueryParameter("name", "masz_backend");

		var response = await restClient.ExecuteAsync(request);

		return Ok(response.Content);
	}
}