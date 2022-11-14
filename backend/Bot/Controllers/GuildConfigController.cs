using Bot.Abstractions;
using Bot.Data;
using Bot.DTOs;
using Bot.Dynamics;
using Bot.Enums;
using Bot.Exceptions;
using Bot.Models;
using Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bot.Controllers;

[Route("api/v1/guilds")]
public class GuildConfigController : AuthenticatedController
{
    private readonly CachedServices _cachedServices;
    private readonly GuildConfigRepository _guildConfigRepo;
    private readonly IServiceProvider _serviceProvider;
    private readonly SettingsRepository _settingsRepository;

    public GuildConfigController(SettingsRepository settingsRepository, GuildConfigRepository guildConfigRepo,
        IdentityManager identityManager, IServiceProvider serviceProvider, CachedServices cachedServices)
        : base(identityManager, settingsRepository, guildConfigRepo)
    {
        _settingsRepository = settingsRepository;
        _guildConfigRepo = guildConfigRepo;
        _serviceProvider = serviceProvider;
        _cachedServices = cachedServices;
    }

    [HttpGet("{guildId}")]
    public async Task<IActionResult> GetGuildConfig([FromRoute] ulong guildId)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        return Ok(await _guildConfigRepo.GetGuildConfig(guildId));
    }

    [HttpDelete("{guildId}")]
    public async Task<IActionResult> DeleteGuildConfig([FromRoute] ulong guildId, [FromQuery] bool deleteData = false)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        await _guildConfigRepo.RequireGuildRegistered(guildId);

        if (deleteData)
            foreach (var repo in _cachedServices.GetInitializedAuthenticatedClasses<DeleteGuildData>(_serviceProvider,
                         identity))
                await repo.DeleteGuildData(guildId);

        await _guildConfigRepo.DeleteGuildConfig(guildId);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGuildConfig([FromBody] GuildConfigForCreateDto guildConfigForCreateDto,
        [FromQuery] bool importExisting = false)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildConfigForCreateDto.GuildId);

        try
        {
            var alreadyRegistered = await _guildConfigRepo.GetGuildConfig(guildConfigForCreateDto.GuildId);

            if (alreadyRegistered != null)
                throw new GuildAlreadyRegisteredException(guildConfigForCreateDto.GuildId);
        }
        catch (ResourceNotFoundException)
        {
        }
        catch (UnregisteredGuildException)
        {
        }

        var config = await _settingsRepository.GetAppSettings();

        GuildConfig guildConfig = new()
        {
            GuildId = guildConfigForCreateDto.GuildId,
            ModRoles = guildConfigForCreateDto.ModRoles,
            AdminRoles = guildConfigForCreateDto.AdminRoles,
            ModNotificationDm = guildConfigForCreateDto.ModNotificationDm,
            StaffChannels = guildConfigForCreateDto.StaffChannels,
            BotChannels = guildConfigForCreateDto.BotChannels,
            StaffAnnouncements = guildConfigForCreateDto.StaffAnnouncements,
            StaffLogs = guildConfigForCreateDto.StaffLogs,
            StrictModPermissionCheck = guildConfigForCreateDto.StrictModPermissionCheck,
            ExecuteWhoIsOnJoin = guildConfigForCreateDto.ExecuteWhoIsOnJoin,
            PublishModeratorInfo = guildConfigForCreateDto.PublishModeratorInfo,
            PreferredLanguage = guildConfigForCreateDto?.PreferredLanguage ?? config.DefaultLanguage
        };

        guildConfig = await _guildConfigRepo.CreateGuildConfig(guildConfig, importExisting);

        return StatusCode(201, guildConfig);
    }

    [HttpPut("{guildId}")]
    public async Task<IActionResult> UpdateGuildConfig([FromRoute] ulong guildId,
        [FromBody] GuildConfigForPutDto newValue)
    {
        var identity = await SetupAuthentication();

        await identity.RequirePermission(DiscordPermission.Admin, guildId);

        var guildConfig = await _guildConfigRepo.GetGuildConfig(guildId);

        guildConfig.ModRoles = newValue.ModRoles;
        guildConfig.AdminRoles = newValue.AdminRoles;

        guildConfig.StaffChannels = newValue.StaffChannels;
        guildConfig.BotChannels = newValue.BotChannels;

        guildConfig.ModNotificationDm = newValue.ModNotificationDm;

        guildConfig.StaffAnnouncements = newValue.StaffAnnouncements;
        guildConfig.StaffLogs = newValue.StaffLogs;

        guildConfig.StrictModPermissionCheck = newValue.StrictModPermissionCheck;
        guildConfig.ExecuteWhoIsOnJoin = newValue.ExecuteWhoIsOnJoin;
        guildConfig.PublishModeratorInfo = newValue.PublishModeratorInfo;
        guildConfig.PreferredLanguage = newValue.PreferredLanguage;

        return Ok(await _guildConfigRepo.UpdateGuildConfig(guildConfig));
    }
}