using Bot.Abstractions;
using Bot.Data;
using Bot.Exceptions;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Humanizer;
using Levels.Data;
using Levels.Events;
using Levels.Models;
using Levels.Services;

namespace Levels.Commands;

public class UpdateRoles : Command<UpdateRoles>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
	public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
	public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }
	public LevelingService? LevelingService { get; set; }
	public DiscordRest? _client { get; set; }

	[SlashCommand("updateroles", "Update a user's roles to match their level.", runMode: RunMode.Async)]
	public async Task RankCommand(
		[Summary("user", "User to update roles for. Defaults to oneself.")] IGuildUser? user = null
		)
	{
		if (Context.Channel is not IGuildChannel)
		{
			await DeclineCommand("This command must be executed in a guild context.");
			return;
		}

		user ??= Context.Guild.GetUser(Context.User.Id);

		if (user is null)
		{
			await RespondAsync("Unable to find guild user. Are you using this command in a registered guild?", ephemeral: true);
			return;
		}

		if (user != Context.User)
		{
			try
			{
				await Identity.RequirePermission(Bot.Enums.DiscordPermission.Moderator, Context.Guild.Id);
			}
			catch (UnauthorizedException)
			{
				await RespondAsync("You must be staff to use this command on a different user. The target user can use this command without a given user to update their own roles.", ephemeral: true);
				return;
			}
			catch (Exception e)
			{
				await RespondAsync("An unexpected error occurred while processing the command: " + e.Message, ephemeral: true);
				return;
			}
		}

		await DeferAsync(true);
		var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
		var guildlevelconfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
		var calclevel = new CalculatedGuildUserLevel(level, guildlevelconfig);

		int totalLevel = calclevel.Total.Level;
		var result = await LevelingService.HandleLevelRoles(level, totalLevel, user, Context.Channel, GuildLevelConfigRepository);
		
		await FollowupAsync(result, ephemeral: true);
	}
}