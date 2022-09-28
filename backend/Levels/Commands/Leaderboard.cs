using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Exceptions;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;

namespace Levels.Commands;

public class Leaderboard : Command<Leaderboard>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }

	[SlashCommand("leaderboard", "Get a link to the Dexter leaderboard")]
	[BotChannel]
	public async Task LeaderboardCommand()
	{
		if (GuildConfigRepository is null)
		{
			Logger.LogError(new NullReferenceException(), "GuildConfigRepository is null");
			throw new UnauthorizedException("GuildConfigRepository is not set in leaderboard.");
		}

		if (Context.Channel is not IGuildChannel)
			throw new UnauthorizedException("This command must be used inside a guild!");

		var settings = await SettingsRepository!.GetAppSettings();
		await RespondAsync($"{settings.GetServiceUrl().Replace("5565", "4200")}/guilds/{Context.Guild.Id}/leaderboard");
	}
}
