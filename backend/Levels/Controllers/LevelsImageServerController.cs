using Bot.Abstractions;
using Bot.DTOs;
using Bot.Exceptions;
using Bot.Models;
using Bot.Services;
using Levels.Data;
using Levels.DTOs;
using Levels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Controllers;

[Route("api/v1/levels")]
public class LevelsImageServerController : BaseController
{
	private readonly LevelsImageRepository _levelsImageRepository;
	private readonly UserRankcardConfigRepository _levelsRankcardRepository;

	public LevelsImageServerController(LevelsImageRepository levelsImageRepository, UserRankcardConfigRepository levelsRankcardRepository)
	{
		_levelsImageRepository = levelsImageRepository;
		_levelsRankcardRepository = levelsRankcardRepository;
	}

	[HttpGet("{userId}/images/{fileName}")]
	public async Task<IActionResult> GetImage([FromRoute] ulong userId, [FromRoute] string fileName)
	{
		UploadedFile? fileInfo;
		try
		{
			fileInfo = await _levelsImageRepository.GetUserFile(userId, fileName);
		}
		catch (ResourceNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return SendImage(fileInfo);
	}

	[HttpGet("default/images/{fileName}")]
	public async Task<IActionResult> GetDefaultImage([FromRoute] string fileName)
	{
		UploadedFile? fileInfo;
		try
		{
			fileInfo = await _levelsImageRepository.GetDefaultFile(fileName);
		}
		catch (ResourceNotFoundException e)
		{
			return NotFound(e.Message);
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return SendImage(fileInfo);
	}

	private IActionResult SendImage(UploadedFile fileInfo)
	{
		HttpContext.Response.Headers.Add("Content-Disposition", fileInfo.ContentDisposition.ToString());
		HttpContext.Response.Headers.Add("Content-Type", fileInfo.ContentType);

		return File(fileInfo.FileContent, fileInfo.ContentType);
	}

	[HttpGet("default/images")]
	public async Task<IActionResult> GetDefaultImages()
	{
		return Ok(await _levelsImageRepository.GetDefaultFiles());
	}
}
