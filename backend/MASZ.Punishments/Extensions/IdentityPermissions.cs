using Discord;
using MASZ.Bot.Abstractions;
using MASZ.Bot.Data;
using MASZ.Bot.Enums;
using MASZ.Bot.Exceptions;
using MASZ.Punishments.Enums;
using MASZ.Punishments.Exceptions;
using MASZ.Punishments.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MASZ.Punishments.Extensions;

public static class IdentityPermissions
{
	public static async Task RequirePermission(this Identity identity, ApiActionPermission permission,
		ModCase modCase)
	{
		if (modCase == null)
			throw new ResourceNotFoundException();

		if (!await identity.HasPermission(permission, modCase))
			throw new UnauthorizedException();

		if (modCase.MarkedToDeleteAt != null && permission == ApiActionPermission.Edit)
			throw new CaseMarkedToBeDeletedException();
	}

	public static async Task<bool> HasPermission(this Identity identity, ApiActionPermission permission,
		ModCase modCase)
	{
		if (modCase == null)
			return false;

		if (await identity.IsSiteAdmin())
			return true;

		var currentUser = identity.GetCurrentUser();

		switch (permission)
		{
			case ApiActionPermission.View:
				if (currentUser == null)
					return false;

				return modCase.UserId == currentUser.Id ||
				       await identity.HasPermission(DiscordPermission.Moderator, modCase.GuildId);
			case ApiActionPermission.Delete:
				return await identity.HasPermission(DiscordPermission.Moderator, modCase.GuildId);
			case ApiActionPermission.ForceDelete:
				return false;
			case ApiActionPermission.Edit:
				try
				{
					using var scope = identity.ServiceProvider.CreateScope();
					var guildConfig = await scope.ServiceProvider.GetRequiredService<GuildConfigRepository>()
						.GetGuildConfig(modCase.GuildId);

					if (!guildConfig.StrictModPermissionCheck || modCase.PunishmentType == PunishmentType.Warn)
						return await identity.HasPermission(DiscordPermission.Moderator, modCase.GuildId);

					var x = modCase.PunishmentType switch
					{
						PunishmentType.Kick => GuildPermission.KickMembers,
						PunishmentType.Ban => GuildPermission.BanMembers,
						PunishmentType.Mute => GuildPermission.ManageRoles,
						_ => GuildPermission.CreateInstantInvite
					};

					if (await identity.HasPermission(DiscordPermission.Admin, modCase.GuildId))
						return true;

					return await identity.HasPermission(DiscordPermission.Moderator, modCase.GuildId) &&
					       await identity.HasPermission(x, modCase.GuildId);

				}
				catch (ResourceNotFoundException)
				{
					return false;
				}
			default:
				throw new ArgumentOutOfRangeException(nameof(permission), permission, null);
		}
	}

	public static async Task RequirePermission(this Identity identity, ApiActionPermission permission,
		ModCaseTemplate caseTemplate)
	{
		if (!await identity.HasPermission(permission, caseTemplate))
			throw new UnauthorizedException();
	}

	public static async Task<bool> HasPermission(this Identity identity, ApiActionPermission permission,
		ModCaseTemplate caseTemplate)
	{
		var currentUser = identity.GetCurrentUser();

		if (currentUser == null)
			return false;

		if (caseTemplate == null)
			return false;

		if (await identity.IsSiteAdmin())
			return true;

		switch (permission)
		{
			case ApiActionPermission.View:
				if (caseTemplate.UserId == currentUser.Id)
					return true;

				return caseTemplate.ViewPermission switch
				{
					ViewPermission.Self => false,
					ViewPermission.Global => true,
					_ => await identity.HasPermission(DiscordPermission.Moderator, caseTemplate.CreatedForGuildId)
				};
			case ApiActionPermission.Edit:
			case ApiActionPermission.Delete:
				return await identity.HasPermission(DiscordPermission.Moderator,
					caseTemplate.CreatedForGuildId) && caseTemplate.UserId == currentUser.Id;
			case ApiActionPermission.ForceDelete:
				return false;
			default:
				return false;
		}
	}
}