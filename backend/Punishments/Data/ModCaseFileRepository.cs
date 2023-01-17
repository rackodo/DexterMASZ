using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Extensions;
using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Punishments.Events;
using System.Net.Mime;

namespace Punishments.Data;

public class ModCaseFileRepository : Repository
{
    private readonly SettingsRepository _configRepo;
    private readonly PunishmentEventHandler _eventHandler;
    private readonly ILogger<ModCaseFileRepository> _logger;
    private readonly ModCaseRepository _modCaseRepository;

    public ModCaseFileRepository(SettingsRepository configRepo,
        PunishmentEventHandler eventHandler, ILogger<ModCaseFileRepository> logger,
        ModCaseRepository modCaseRepository, DiscordRest discordRest) : base(discordRest)
    {
        _configRepo = configRepo;
        _eventHandler = eventHandler;
        _logger = logger;
        _modCaseRepository = modCaseRepository;

        _configRepo.AsUser(Identity);
        _modCaseRepository.AsUser(Identity);
    }

    public async Task<UploadedFile> GetCaseFile(ulong guildId, int caseId, string fileName)
    {
        var config = await _configRepo.GetAppSettings();

        var filePath = Path.Join(config.AbsolutePathToFileUpload, guildId.ToString(), caseId.ToString(),
            FilesHandler.RemoveSpecialCharacters(fileName));

        var fullFilePath = Path.GetFullPath(filePath);

        // https://stackoverflow.com/a/1321535/9850709
        if (fullFilePath != filePath)
            throw new InvalidPathException();

        byte[] fileData;

        try
        {
            fileData = FilesHandler.ReadFile(filePath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to read file");
            throw new ResourceNotFoundException();
        }

        if (fileData == null)
            throw new ResourceNotFoundException();

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

    public async Task<List<string>> GetCaseFiles(ulong guildId, int caseId)
    {
        var config = await _configRepo.GetAppSettings();

        var uploadDir = Path.Join(config.AbsolutePathToFileUpload, guildId.ToString(), caseId.ToString());

        var fullPath = Path.GetFullPath(uploadDir);

        // https://stackoverflow.com/a/1321535/9850709
        if (fullPath != uploadDir)
            throw new InvalidPathException();

        var files = FilesHandler.GetFilesByDirectory(fullPath);

        return files == null ? throw new ResourceNotFoundException() : files.Select(f => f.Name).ToList();
    }

    public async Task<string> UploadFile(IFormFile file, ulong guildId, int caseId)
    {
        var config = await _configRepo.GetAppSettings();

        var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

        var uploadDir = Path.Join(config.AbsolutePathToFileUpload, guildId.ToString(), caseId.ToString());

        var fullPath = Path.GetFullPath(uploadDir);

        // https://stackoverflow.com/a/1321535/9850709
        if (fullPath != uploadDir)
            throw new InvalidPathException();

        var fileName = await FilesHandler.SaveFile(file, fullPath);

        _eventHandler.FileUploadedEvent.Invoke(await GetCaseFile(guildId, caseId, fileName), modCase, Identity);

        return fileName;
    }

    public async Task DeleteFile(ulong guildId, int caseId, string fileName)
    {
        var config = await _configRepo.GetAppSettings();

        var modCase = await _modCaseRepository.GetModCase(guildId, caseId);

        var info = await GetCaseFile(guildId, caseId, fileName);

        var filePath = Path.Join(config.AbsolutePathToFileUpload, guildId.ToString(), caseId.ToString(),
            FilesHandler.RemoveSpecialCharacters(fileName));

        var fullFilePath = Path.GetFullPath(filePath);

        // https://stackoverflow.com/a/1321535/9850709
        if (fullFilePath != filePath)
            throw new InvalidPathException();

        if (!FilesHandler.FileExists(fullFilePath))
            throw new ResourceNotFoundException();

        FilesHandler.DeleteFile(fullFilePath);

        _eventHandler.FileDeletedEvent.Invoke(info, modCase, Identity);
    }
}
