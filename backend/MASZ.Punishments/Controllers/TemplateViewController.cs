using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.Models;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Punishments.Controllers;

[Route("api/v1/templatesview")]
public class TemplateViewController : AuthenticatedController
{
	private readonly DiscordRest _discordRest;
	private readonly CaseTemplateRepository _templateRepository;

	public TemplateViewController(CaseTemplateRepository templateRepository, DiscordRest discordRest,
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

		foreach (var template in templates.Where(x => x.UserId == userId))
			templatesView.Add(new ModCaseTemplateExpanded(
				template,
				await _discordRest.FetchUserInfo(template.UserId, CacheBehavior.OnlyCache),
				_discordRest.FetchGuildInfo(template.CreatedForGuildId, CacheBehavior.Default)
			));

		return Ok(templatesView);
	}
}