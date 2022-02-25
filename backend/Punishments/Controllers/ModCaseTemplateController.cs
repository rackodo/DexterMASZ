using Bot.Abstractions;
using Bot.Enums;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;
using Punishments.Data;
using Punishments.DTOs;
using Punishments.Extensions;
using Punishments.Models;

namespace Punishments.Controllers;

[Route("api/v1/templates")]
public class ModCaseTemplateController : AuthenticatedController
{
	private readonly ModCaseTemplateRepository _caseTemplateRepo;

	public ModCaseTemplateController(ModCaseTemplateRepository caseTemplateRepo, IdentityManager identityManager) :
		base(identityManager, caseTemplateRepo)
	{
		_caseTemplateRepo = caseTemplateRepo;
	}

	[HttpPost]
	public async Task<IActionResult> CreateTemplate([FromBody] ModCaseTemplateForCreateDto templateDto,
		[FromQuery] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var template = new ModCaseTemplate
		{
			TemplateName = templateDto.TemplateName,
			UserId = identity.GetCurrentUser().Id,
			ViewPermission = templateDto.ViewPermission,
			CreatedForGuildId = guildId,
			CreatedAt = DateTime.UtcNow,
			CaseTitle = templateDto.Title,
			CaseDescription = templateDto.Description,
			CaseLabels = templateDto.Labels,
			CasePunishedUntil = templateDto.PunishedUntil,
			CasePunishmentType = templateDto.PunishmentType,
			HandlePunishment = templateDto.HandlePunishment
		};

		var createdTemplate = await _caseTemplateRepo.CreateTemplate(template);

		return StatusCode(201, createdTemplate);
	}

	[HttpDelete("{templateId}")]
	public async Task<IActionResult> DeleteTemplate([FromRoute] int templateId)
	{
		var identity = await SetupAuthentication();

		var template = await _caseTemplateRepo.GetTemplate(templateId);

		await identity.RequirePermission(ApiActionPermission.Delete, template);

		await _caseTemplateRepo.DeleteTemplate(template);

		return Ok(template);
	}
}