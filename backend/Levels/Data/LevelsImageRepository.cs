using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace Levels.Data;

public class LevelsImageRepository : Repository
{
	private readonly SettingsRepository _configRepo;
	private readonly ILogger<LevelsImageRepository> _logger;
	private readonly GuildUserLevelRepository _levelsRepository;

	public LevelsImageRepository(SettingsRepository configRepo, ILogger<LevelsImageRepository> logger,
		GuildUserLevelRepository modCaseRepository, DiscordRest discordRest) : base(discordRest)
	{
		_configRepo = configRepo;
		_logger = logger;
		_levelsRepository = modCaseRepository;

		_configRepo.AsUser(Identity);
		_levelsRepository.AsUser(Identity);
	}

	private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

	public async Task<string> GetUserUploadDir(ulong userId)
	{
		var config = await _configRepo.GetAppSettings();

		return Path.Join(config.AbsolutePathToFileUpload, "RankBackgrounds", userId.ToString());
	}

	public static string GetDefaultBackgroundDir() =>
		Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", "Leveling", "Backgrounds");

	public async Task<List<string>> GetUserFiles(ulong userId) =>
		GetFilesInLevelsDirectory(await GetUserUploadDir(userId));

	public static List<string> GetDefaultFiles() =>
		GetFilesInLevelsDirectory(GetDefaultBackgroundDir());

	public async Task<UploadedFile> GetUserFile(ulong userId, string fileName) =>
		GetSpecificFile(await GetUserUploadDir(userId), fileName, $"User {userId} has no image file with filename \"{fileName}\"");

	public UploadedFile GetDefaultFile(string fileName) =>
		GetSpecificFile(GetDefaultBackgroundDir(), fileName, $"No default image file with filename \"{fileName}\"");

	private UploadedFile GetSpecificFile(string dir, string fileName, string notFoundDescription = "")
	{
		byte[] fileData;

		var filePath = Path.Join(dir, FilesHandler.RemoveSpecialCharacters(fileName));

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

	private static List<string> GetFilesInLevelsDirectory(string uploadDir)
	{
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
		var uploadDir = await GetUserUploadDir(userId);

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
		var uploadDir = await GetUserUploadDir(userId);

		var filePath = Path.Join(uploadDir, FilesHandler.RemoveSpecialCharacters(fileName));

		var fullFilePath = Path.GetFullPath(filePath);

		// https://stackoverflow.com/a/1321535/9850709
		if (fullFilePath != filePath)
			throw new InvalidPathException();

		if (!FilesHandler.FileExists(fullFilePath))
			throw new ResourceNotFoundException();

		FilesHandler.DeleteFile(fullFilePath);
	}
}
