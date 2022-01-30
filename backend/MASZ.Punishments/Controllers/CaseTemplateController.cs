using MASZ.Bot.Abstractions;
using MASZ.Bot.Enums;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.DTOs;
using MASZ.Punishments.Extensions;
using MASZ.Punishments.Models;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Punishments.Controllers;

[Route("api/v1/templates")]
public class CaseTemplateController : AuthenticatedController
{
	private readonly CaseTemplateRepository _caseTemplateRepo;

	public CaseTemplateController(CaseTemplateRepository caseTemplateRepo, IdentityManager identityManager) :
		base(identityManager, caseTemplateRepo)
	{
		_caseTemplateRepo = caseTemplateRepo;
	}

	[HttpPost]
	public async Task<IActionResult> CreateTemplate([FromBody] CaseTemplateForCreateDto templateDto,
		[FromQuery] ulong guildId)
	{
		var identity = await SetupAuthentication();

		await identity.RequirePermission(DiscordPermission.Moderator, guildId);

		var template = new CaseTemplate
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
			SendPublicNotification = templateDto.SendPublicNotification,
			AnnounceDm = templateDto.AnnounceDm,
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