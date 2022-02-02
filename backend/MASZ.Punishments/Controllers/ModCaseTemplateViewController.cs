using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.Models;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Punishments.Controllers;

[Route("api/v1/templatesview")]
public class ModCaseTemplateViewController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly ModCaseTemplateRepository _templateRepository;

	public ModCaseTemplateViewController(ModCaseTemplateRepository templateRepository, DiscordRest discordRest,
		IdentityManager identityManager) :
		base(identityManager, templateRepository)
	{
		_templateRepository = templateRepository;
		_discordRest = discordRest;
	}

	[HttpGet]
	public async Task<IActionResult> GetTemplatesView([FromQuery] ulong userId = 0)
	{
		var identity = await SetupAuthentication();

		var templates = await _templateRepository.GetTemplatesBasedOnPermissions(identity);
		var templatesView = new List<ModCaseTemplateExpanded>();

		foreach (var template in templates.Where(x => x.UserId == userId || userId == 0))
			templatesView.Add(new ModCaseTemplateExpanded(
				template,
				await _discordRest.FetchUserInfo(template.UserId, CacheBehavior.OnlyCache),
				_discordRest.FetchGuildInfo(template.CreatedForGuildId, CacheBehavior.Default)
			));

		return Ok(templatesView);
	}
}