using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Enums;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.Commands;

public class Leaderboard : Command<Leaderboard>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }

	[SlashCommand("leaderboard", "Get a link to the Dexter leaderboard")]
	public async Task LeaderboardCommand()
	{
		if (GuildConfigRepository is null)
		{
			Logger.LogError(new NullReferenceException(), "GuildConfigRepository is null");
			await DeclineCommand("GuildConfigRepository is not set in Leaderboard.");
			return;
		}
		if (Context.Channel is not IGuildChannel)
		{
			await DeclineCommand("This command must be used inside a guild!");
			return;
		}
		GuildConfigRepository.AsUser(Identity);
		var guildConfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);
		if (!await EnsureBotChannel(guildConfig))
			return;

		var settings = await SettingsRepository!.GetAppSettings();
		await RespondAsync($"{settings.GetServiceUrl().Replace("5565", "4200")}/guilds/{Context.Guild.Id}/leaderboard");
	}
}
