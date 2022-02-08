using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Bot.Models;
using Discord;
using Discord.Interactions;
using System.Text;
using Utilities.Translators;

namespace Utilities.Commands;

public class Feature : Command<Feature>
{
	public GuildConfigRepository GuildConfigRepository { get; set; }

	[Require(RequireCheck.GuildModerator)]
	[SlashCommand("features", "Checks if further configuration is needed to use Dexter features.")]
	public async Task FeatureCommand()
	{
		GuildConfigRepository.AsUser(Identity);

		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);

		EmbedBuilder embed = new();
		embed.WithTimestamp(DateTime.UtcNow);

		StringBuilder missingBasicPermissions = new();

		// Kick
		missingBasicPermissions.Append(
			HasKickPermission(Context.Guild.CurrentUser)
				? $"\n- ✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesKickPermissionGranted()}"
				: $"\n- ❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesKickPermissionNotGranted()}");

		// Ban
		missingBasicPermissions.Append(
			HasBanPermission(Context.Guild.CurrentUser)
				? $"\n- ✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesBanPermissionGranted()}"
				: $"\n- ❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesBanPermissionNotGranted()}");

		// Mute
		missingBasicPermissions.Append(
			HasManagedRolePermission(Context.Guild.CurrentUser)
				? $"\n- ✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesManageRolePermissionGranted()}"
				: $"\n- ❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesManageRolePermissionNotGranted()}");

		// Basic punishment feature
		if (FeaturePunishmentExecution(Context.Guild.CurrentUser))
			embed.AddField($"✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesPunishmentExecution()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesPunishmentExecutionDescription());
		else
			embed.AddField($"❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesPunishmentExecution()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesPunishmentExecutionDescription() +
				missingBasicPermissions);

		// Unban feature
		if (HasBanPermission(Context.Guild.CurrentUser))
			embed.AddField($"✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesUnbanRequests()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesUnbanRequestsDescriptionGranted());
		else
			embed.AddField($"❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesUnbanRequests()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesUnbanRequestsDescriptionNotGranted());

		// Report command
		if (HasWebhookDefined(guildConfig))
			embed.AddField($"✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesReportCommand()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesReportCommandDescriptionGranted());
		else
			embed.AddField($"❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesReportCommand()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesReportCommandDescriptionNotGranted());

		// Invite tracking
		if (HasManagedGuildPermission(Context.Guild.CurrentUser))
			embed.AddField($"✅ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesInviteTracking()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesInviteTrackingDescriptionGranted());
		else
			embed.AddField($"❌ {Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesInviteTracking()}",
				Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesInviteTrackingDescriptionNotGranted());

		if (SupportsAllFeatures(Context.Guild.CurrentUser, guildConfig))
			embed.WithTitle(Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesSupportAllFeatures())
				.WithDescription(Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesSupportAllFeaturesDesc())
				.WithColor(Color.Green);
		else
			embed.WithTitle(Translator.Get<UtilityFeaturesTranslator>().CmdFeaturesMissingFeatures())
				.WithColor(Color.Red);

		await Context.Interaction.RespondAsync(embed: embed.Build());
	}

	public static bool HasWebhookDefined(GuildConfig guildConfig)
	{
		return !string.IsNullOrEmpty(guildConfig.ModNotificationWebhook);
	}

	public static bool HasKickPermission(IGuildUser user)
	{
		return user.GuildPermissions.KickMembers;
	}

	public static bool HasBanPermission(IGuildUser user)
	{
		return user.GuildPermissions.BanMembers;
	}

	public static bool HasManagedRolePermission(IGuildUser user)
	{
		return user.GuildPermissions.ManageRoles;
	}

	public static bool HasManagedGuildPermission(IGuildUser user)
	{
		return user.GuildPermissions.ManageGuild;
	}

	public static bool FeaturePunishmentExecution(IGuildUser user)
	{
		return HasKickPermission(user) &&
			   HasBanPermission(user) &&
			   HasManagedRolePermission(user) &&
			   HasManagedGuildPermission(user);
	}

	public static bool SupportsAllFeatures(IGuildUser user, GuildConfig guildConfig)
	{
		return HasKickPermission(user) &&
			   HasBanPermission(user) &&
			   HasManagedRolePermission(user) &&
			   HasManagedGuildPermission(user) &&
			   HasWebhookDefined(guildConfig);
	}
}