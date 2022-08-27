using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Models;
using Bot.Services;
using Levels.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace Levels.Data;

public class LevelsImageRepository : Repository
{
	private readonly SettingsRepository _configRepo;
	private readonly LevelsEventHandler _eventHandler;
	private readonly ILogger<LevelsImageRepository> _logger;
	private readonly GuildUserLevelRepository _levelsRepository;

	public LevelsImageRepository(SettingsRepository configRepo,
		LevelsEventHandler eventHandler, ILogger<LevelsImageRepository> logger,
		GuildUserLevelRepository modCaseRepository, DiscordRest discordRest) : base(discordRest)
	{
		_configRepo = configRepo;
		_eventHandler = eventHandler;
		_logger = logger;
		_levelsRepository = modCaseRepository;

		_configRepo.AsUser(Identity);
		_levelsRepository.AsUser(Identity);
	}

	private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

	public async Task<UploadedFile> GetUserFile(ulong userId, string fileName)
	{
		return await GetSpecificFile(userId.ToString(), fileName, $"User {userId} has no image file with filename \"{fileName}\"");
	}

	public async Task<UploadedFile> GetDefaultFile(string fileName)
	{
		return await GetSpecificFile("_DEFAULT", fileName, $"No default image file with filename \"{fileName}\"");
	}

	private async Task<UploadedFile> GetSpecificFile(string dir, string fileName, string notFoundDescription = "")
	{
		var config = await _configRepo.GetAppSettings();

		byte[] fileData;

		var filePath = Path.Join(config.AbsolutePathToFileUpload, "levels", dir,
			FilesHandler.RemoveSpecialCharacters(fileName));

		var fullFilePath = Path.GetFullPath(filePath);

		// https://stackoverflow.com/a/1321535/9850709
		if (fullFilePath != filePath)
			throw new InvalidPathException();

		try
		{
			fileData = FilesHandler.ReadFile(filePath);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Failed to read file");
			throw new BadHttpRequestException(e.Message);
		}

		if (fileData == null)
			throw new ResourceNotFoundException(notFoundDescription);

		var contentType = FilesHandler.GetContentType(filePath);

		var cd = new ContentDisposition
		{
			FileName = fileName,
			Inline = true
		};

		return new UploadedFile
		{
			Name = fileName,
			ContentType = contentType,
			ContentDisposition = cd,
			FileContent = fileData
		};
	}

	public async Task<List<string>> GetUserFiles(ulong userId)
	{
		return await GetFilesInLevelsDirectory(userId.ToString());
	}

	public async Task<List<string>> GetDefaultFiles()
	{
		return await GetFilesInLevelsDirectory("_DEFAULT");
	}

	private async Task<List<string>> GetFilesInLevelsDirectory(string dir)
	{
		var config = await _configRepo.GetAppSettings();

		var uploadDir = Path.Join(config.AbsolutePathToFileUpload, "levels", dir);

		var fullPath = Path.GetFullPath(uploadDir);

		// https://stackoverflow.com/a/1321535/9850709
		if (fullPath != uploadDir)
			throw new InvalidPathException();

		var files = FilesHandler.GetFilesByDirectory(fullPath);

		if (files == null)
			throw new ResourceNotFoundException();

		return files.Select(f => f.Name).ToList();
	}

	public async Task<string> UploadFile(ulong userId, IFormFile file)
	{
		var config = await _configRepo.GetAppSettings();

		var uploadDir = Path.Join(config.AbsolutePathToFileUpload, "levels", userId.ToString());

		var ext = Path.GetExtension(file.FileName);
		if (!AllowedExtensions.Contains(ext))
		{
			throw new BadImageFormatException($"Received file with invalid extension \"{ext}\". " +
				$"Extension must be one of the following: {string.Join(", ", AllowedExtensions)}");
		}

		var fullPath = Path.GetFullPath(uploadDir);

		// https://stackoverflow.com/a/1321535/9850709
		if (fullPath != uploadDir)
			throw new InvalidPathException();

		var fileName = await FilesHandler.SaveFile(file, uploadDir);

		return Path.Combine(uploadDir, fileName);
	}

	public async Task DeleteFile(ulong userId, string fileName)
	{
		var config = await _configRepo.GetAppSettings();

		var info = await GetUserFile(userId, fileName);

		var filePath = Path.Join(config.AbsolutePathToFileUpload, "levels", userId.ToString(),
			FilesHandler.RemoveSpecialCharacters(fileName));

		var fullFilePath = Path.GetFullPath(filePath);

		// https://stackoverflow.com/a/1321535/9850709
		if (fullFilePath != filePath)
			throw new InvalidPathException();

		if (!FilesHandler.FileExists(fullFilePath))
			throw new ResourceNotFoundException();

		FilesHandler.DeleteFile(fullFilePath);
	}
}
