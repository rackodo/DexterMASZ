using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.DTOs;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Services;
using MASZ.Punishments.Data;
using MASZ.Punishments.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Punishments.Controllers;

[Route("api/v1/guilds/{guildId}/cases/{caseId}/files")]
public class ModCaseFileController : AuthenticatedController
{
	private readonly CaseFileRepository _caseFileRepository;
	private readonly ModCaseRepository _modCaseRepository;
	private readonly SettingsRepository _settingsRepository;

	public ModCaseFileController(IdentityManager identityManager, CaseFileRepository caseFileRepository,
		ModCaseRepository modCaseRepository, SettingsRepository settingsRepository) :
		base(identityManager, caseFileRepository, settingsRepository, modCaseRepository)
	{
		_caseFileRepository = caseFileRepository;
		_settingsRepository = settingsRepository;
		_modCaseRepository = modCaseRepository;
	}

	[Authorize]
	[HttpDelete("{filename}")]
	public async Task<IActionResult> DeleteSpecificItem([FromRoute] ulong guildId, [FromRoute] int caseId,
		[FromRoute] string filename)
	{
		var identity = await SetupAuthentication();

		var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

		await identity.RequirePermission(ApiActionPermission.Edit, modCase);

		await _caseFileRepository.DeleteFile(guildId, caseId, filename);

		return Ok();
	}

	[HttpGet("{filename}")]
	public async Task<IActionResult> GetSpecificItem([FromRoute] ulong guildId, [FromRoute] int caseId,
		[FromRoute] string filename)
	{
		var identity = await SetupAuthentication();

		var config = await _settingsRepository.GetAppSettings();

		if (!config.PublicFileMode)
		{
			var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

			await identity.RequirePermission(ApiActionPermission.View, modCase);
		}

		var fileInfo = await _caseFileRepository.GetCaseFile(guildId, caseId, filename);

		HttpContext.Response.Headers.Add("Content-Disposition", fileInfo.ContentDisposition.ToString());
		HttpContext.Response.Headers.Add("Content-Type", fileInfo.ContentType);

		return File(fileInfo.FileContent, fileInfo.ContentType);
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetAllItems([FromRoute] ulong guildId, [FromRoute] int caseId)
	{
		var identity = await SetupAuthentication();

		var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

		await identity.RequirePermission(ApiActionPermission.View, modCase);

		var files = new List<string>();

		try
		{
			files = await _caseFileRepository.GetCaseFiles(guildId, caseId);
		}
		catch (ResourceNotFoundException)
		{
		}

		return Ok(new { names = files });
	}

	[Authorize]
	[HttpPost]
	[RequestSizeLimit(10485760)]
	public async Task<IActionResult> PostItem([FromRoute] ulong guildId, [FromRoute] int caseId,
		[FromForm] UploadedFileDto uploadedFile)
	{
		var identity = await SetupAuthentication();

		var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

		await identity.RequirePermission(ApiActionPermission.Edit, modCase);

		return Ok(new { path = await _caseFileRepository.UploadFile(uploadedFile.File, guildId, caseId) });
	}
}