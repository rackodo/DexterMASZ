using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.DTOs;
using MASZ.Bot.Dynamics;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Bot.Models;
using MASZ.Bot.Services;
using Microsoft.AspNetCore.Mvc;

namespace MASZ.Bot.Controllers;

[Route("api/v1/guilds")]
public class GuildConfigController : AuthenticatedController
{
	private readonly GuildConfigRepository _guildConfigRepo;
	private readonly CachedServices _cachedServices;
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

		await identity.RequireSiteAdmin();

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
			MutedRoles = guildConfigForCreateDto.MutedRoles,
			ModPublicNotificationWebhook = guildConfigForCreateDto.ModPublicNotificationWebhook,
			ModInternalNotificationWebhook = guildConfigForCreateDto.ModInternalNotificationWebhook,
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

		var config = await _settingsRepository.GetAppSettings();

		if (config.DemoModeEnabled)
			if (!await identity.IsSiteAdmin())
				throw new DemoModeEnabledException();

		var guildConfig = await _guildConfigRepo.GetGuildConfig(guildId);

		guildConfig.ModRoles = newValue.ModRoles;
		guildConfig.AdminRoles = newValue.AdminRoles;
		guildConfig.ModNotificationDm = newValue.ModNotificationDm;
		guildConfig.MutedRoles = newValue.MutedRoles;

		guildConfig.ModInternalNotificationWebhook = newValue.ModInternalNotificationWebhook;

		if (guildConfig.ModInternalNotificationWebhook != null)
			guildConfig.ModInternalNotificationWebhook =
				guildConfig.ModInternalNotificationWebhook.Replace("discord.com", "discordapp.com");

		guildConfig.ModPublicNotificationWebhook = newValue.ModPublicNotificationWebhook;

		if (guildConfig.ModPublicNotificationWebhook != null)
			guildConfig.ModPublicNotificationWebhook =
				guildConfig.ModPublicNotificationWebhook.Replace("discord.com", "discordapp.com");

		guildConfig.StrictModPermissionCheck = newValue.StrictModPermissionCheck;
		guildConfig.ExecuteWhoIsOnJoin = newValue.ExecuteWhoIsOnJoin;
		guildConfig.PublishModeratorInfo = newValue.PublishModeratorInfo;
		guildConfig.PreferredLanguage = newValue.PreferredLanguage;

		return Ok(await _guildConfigRepo.UpdateGuildConfig(guildConfig));
	}
}