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

public class Rankcard : Command<Rankcard>
{
	public SettingsRepository? SettingsRepository { get; set; }

	[SlashCommand("rankcard", "Customize your rankcard.")]
	public async Task LeaderboardCommand()
	{
		var settings = await SettingsRepository!.GetAppSettings();
		await RespondAsync($"{settings.GetServiceUrl().Replace("5565", "4200")}/profile?selectedTab=rankcard");
	}
}
