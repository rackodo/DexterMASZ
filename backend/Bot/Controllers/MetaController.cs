using Bot.Abstractions;
using Bot.Data;
using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Bot.Controllers;

[Route("api/v1/meta")]
public class MetaController : BaseController
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
		return Ok(new DiscordUser(_discordRest.GetCurrentBotInfo()));
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
		return Ok(new DiscordApplication(await _discordRest.GetCurrentApplicationInfo()));
	}

	[HttpGet("versions")]
	public async Task<IActionResult> GetReleases()
	{
		var restClient = new RestClient("https://dexterindex.zaanposni.com/");
		var request = new RestRequest("/api/v1/versions");
		request.AddQueryParameter("name", "dexter_backend");

		var response = await restClient.ExecuteAsync(request);

		return Ok(response.Content);
	}
}