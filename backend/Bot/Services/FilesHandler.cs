using Bot.Data;
using Bot.Dynamics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Bot.Services;

public class FilesHandler : IDeleteGuildData
{
    private readonly ILogger<FilesHandler> _logger;
    private readonly IServiceProvider _services;

    public FilesHandler(ILogger<FilesHandler> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    public async Task DeleteGuildData(ulong guildId)
    {
        using var scope = _services.CreateScope();
        var config = await scope.ServiceProvider.GetRequiredService<SettingsRepository>().GetAppSettings();

        try
        {
            DeleteDirectory(Path.Combine(config.AbsolutePathToFileUpload, guildId.ToString()));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete files directory for guilds.");
        }
    }

    public static string GetContentType(string path)
    {
        if (!File.Exists(path))
            return "application/octet-stream";

        new FileExtensionContentTypeProvider().TryGetContentType(path, out var contentType);

        return contentType ?? "application/octet-stream";
    }

    public static FileInfo[] GetFilesByDirectory(string directory) =>
        !Directory.Exists(directory) ? null : new DirectoryInfo(directory).GetFiles();

    public static byte[] ReadFile(string path) => !File.Exists(path) ? null : File.ReadAllBytes(path);

    public static async Task<string> SaveFile(IFormFile file, string directory)
    {
        Directory.CreateDirectory(directory);

        var uniqueFileName = GetUniqueFileName(file);
        var filePath = Path.Combine(directory, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return uniqueFileName;
    }

    public static void DeleteDirectory(string directory)
    {
        if (Directory.Exists(directory))
            Directory.Delete(directory, true);
    }

    public static void DeleteFile(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    public static bool FileExists(string path) => File.Exists(path);

    public static string RemoveSpecialCharacters(string str)
    {
        StringBuilder sb = new();

        foreach (var t in str.Where(t => t is >= '0' and <= '9' or >= 'A' and <= 'z' or '.' or '_'))
            sb.Append(t);

        return sb.ToString();
    }

    private static string GetUniqueFileName(IFormFile file)
    {
        var fileName = Path.GetFileName(file.FileName);

        return GetSha1Hash(file)
               + "_"
               + Guid.NewGuid().ToString()[..8]
               + "_"
               + RemoveSpecialCharacters(Path.GetFileNameWithoutExtension(fileName))
               + RemoveSpecialCharacters(Path.GetExtension(fileName));
    }

    private static string GetSha1Hash(IFormFile file)
    {
        // Get stream from file then convert it to a MemoryStream.
        MemoryStream stream = new();
        file.OpenReadStream().CopyTo(stream);

        // Compute md5 hash of the file's byte array.
        var bytes = SHA1.Create().ComputeHash(stream.ToArray());

        stream.Close();
        return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower();
    }
}
