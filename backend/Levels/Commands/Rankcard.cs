using Bot.Abstractions;
using Bot.Data;
using Discord.Interactions;

namespace Levels.Commands;

public class Rankcard : Command<Rankcard>
{
    public SettingsRepository? SettingsRepository { get; set; }

    [SlashCommand("rankcard", "Customize your rankcard.")]
    public async Task LeaderboardCommand()
    {
        var settings = await SettingsRepository!.GetAppSettings();
        await RespondAsync($"{settings.GetServiceUrl().Replace("5565", "4200")}/profile?selectedTab=rankcard",
            ephemeral: true);
    }
}