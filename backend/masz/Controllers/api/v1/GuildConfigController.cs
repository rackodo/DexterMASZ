using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using masz.Dtos.GuildConfig;
using masz.Enums;
using masz.Exceptions;
using masz.Models;
using masz.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace masz.Controllers
{
    [ApiController]
    [Route("api/v1/guilds/")]
    [Authorize]
    public class GuildConfigController : SimpleController
    {
        private readonly ILogger<GuildConfigController> _logger;

        public GuildConfigController(ILogger<GuildConfigController> logger, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = logger;
        }

        [HttpGet("{guildId}")]
        public async Task<IActionResult> GetSpecificItem([FromRoute] ulong guildId)
        {
            await RequirePermission(guildId, DiscordPermission.Admin);

            return Ok(await GuildConfigRepository.CreateDefault(_serviceProvider).GetGuildConfig(guildId));
        }

        [HttpDelete("{guildId}")]
        public async Task<IActionResult> DeleteSpecificItem([FromRoute] ulong guildId, [FromQuery] bool deleteData = false)
        {
            await RequireSiteAdmin();
            await GetRegisteredGuild(guildId);

            await GuildConfigRepository.CreateDefault(_serviceProvider).DeleteGuildConfig(guildId, deleteData);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] GuildConfigForCreateDto guildConfigForCreateDto)
        {
            await RequireSiteAdmin();

            try {
                GuildConfig alreadyRegistered = await GetRegisteredGuild(guildConfigForCreateDto.GuildId);
                if (alreadyRegistered != null)
                {
                    throw new GuildAlreadyRegisteredException(guildConfigForCreateDto.GuildId);
                }
            } catch (ResourceNotFoundException) { }

            DiscordGuild guild = await _discordAPI.FetchGuildInfo(guildConfigForCreateDto.GuildId, CacheBehavior.IgnoreCache);
            if (guild == null)
            {
                return BadRequest("Guild not found.");
            }
            foreach (ulong role in guildConfigForCreateDto.modRoles)
            {
                if (guild.Roles.ContainsKey(role))
                {
                    return BadRequest($"Role {role} not found.");
                }
            }
            foreach (ulong role in guildConfigForCreateDto.adminRoles)
            {
                if (guild.Roles.ContainsKey(role))
                {
                    return BadRequest($"Role {role} not found.");
                }
            }
            foreach (ulong role in guildConfigForCreateDto.mutedRoles)
            {
                if (guild.Roles.ContainsKey(role))
                {
                    return BadRequest($"Role {role} not found.");
                }
            }

            GuildConfig guildConfig = new GuildConfig();
            guildConfig.GuildId = guildConfigForCreateDto.GuildId;
            guildConfig.ModRoles = guildConfigForCreateDto.modRoles.Select(x => x.ToString()).ToArray();
            guildConfig.AdminRoles = guildConfigForCreateDto.adminRoles.Select(x => x.ToString()).ToArray();
            guildConfig.ModNotificationDM = guildConfigForCreateDto.ModNotificationDM;
            guildConfig.MutedRoles = guildConfigForCreateDto.mutedRoles.Select(x => x.ToString()).ToArray();
            guildConfig.ModPublicNotificationWebhook = guildConfigForCreateDto.ModPublicNotificationWebhook;
            if (guildConfig.ModPublicNotificationWebhook != null) {
                guildConfig.ModPublicNotificationWebhook = guildConfig.ModPublicNotificationWebhook.Replace("discord.com", "discordapp.com");
            }
            guildConfig.ModInternalNotificationWebhook = guildConfigForCreateDto.ModInternalNotificationWebhook;
            if (guildConfig.ModInternalNotificationWebhook != null) {
                guildConfig.ModInternalNotificationWebhook = guildConfig.ModInternalNotificationWebhook.Replace("discord.com", "discordapp.com");
            }
            guildConfig.StrictModPermissionCheck = guildConfigForCreateDto.StrictModPermissionCheck;
            guildConfig.ExecuteWhoisOnJoin = guildConfigForCreateDto.ExecuteWhoisOnJoin;
            guildConfig.PublishModeratorInfo = guildConfigForCreateDto.PublishModeratorInfo;

            await GuildConfigRepository.CreateDefault(_serviceProvider).CreateGuildConfig(guildConfig);

            Task task = new Task(() => {
                _scheduler.CacheAll();
            });
            task.Start();

            return StatusCode(201, guildConfig);
        }

        [HttpPut("{guildId}")]
        public async Task<IActionResult> UpdateSpecificItem([FromRoute] ulong guildId, [FromBody] GuildConfigForPutDto newValue)
        {
            await RequirePermission(guildId, DiscordPermission.Admin);
            if (String.Equals("true", System.Environment.GetEnvironmentVariable("ENABLE_DEMO_MODE"))) {
                if (! (await GetIdentity()).IsSiteAdmin()) {  // siteadmins can overwrite in demo mode
                    throw new BaseAPIException("Demo mode is enabled. Only site admins can edit guild configs.", APIError.NotAllowedInDemoMode);
                }
            }
            GuildConfig guildConfig = await GetRegisteredGuild(guildId);

            guildConfig.ModRoles = newValue.ModRoles.Select(x => x.ToString()).ToArray();
            guildConfig.AdminRoles = newValue.AdminRoles.Select(x => x.ToString()).ToArray();
            guildConfig.ModNotificationDM = newValue.ModNotificationDM;
            guildConfig.MutedRoles = newValue.MutedRoles.Select(x => x.ToString()).ToArray();
            guildConfig.ModInternalNotificationWebhook = newValue.ModInternalNotificationWebhook;
            if (guildConfig.ModInternalNotificationWebhook != null) {
                guildConfig.ModInternalNotificationWebhook = guildConfig.ModInternalNotificationWebhook.Replace("discord.com", "discordapp.com");
            }
            guildConfig.ModPublicNotificationWebhook = newValue.ModPublicNotificationWebhook;
            if (guildConfig.ModPublicNotificationWebhook != null) {
                guildConfig.ModPublicNotificationWebhook = guildConfig.ModPublicNotificationWebhook.Replace("discord.com", "discordapp.com");
            }
            guildConfig.StrictModPermissionCheck = newValue.StrictModPermissionCheck;
            guildConfig.ExecuteWhoisOnJoin = newValue.ExecuteWhoisOnJoin;
            guildConfig.PublishModeratorInfo = newValue.PublishModeratorInfo;

            return Ok(await GuildConfigRepository.CreateDefault(_serviceProvider).UpdateGuildConfig(guildConfig));
        }
    }
}