using Bot.Abstractions;
using Bot.Data;
using Discord.Interactions;

namespace Levels.Commands;

public class RankCard : Command<RankCard>
{
    public SettingsRepository? SettingsRepository { get; set; }

    [SlashCommand("rankcard", "Customize your rankcard.")]
    public async Task RankCardCommand() => await RespondAsync(await GetRankCard(SettingsRepository), ephemeral: true);

    public static async Task<string> GetRankCard(SettingsRepository settingRepo) =>
        $"{(await settingRepo!.GetAppSettings()).GetServiceUrl().Replace("5565", "4200")}/profile?selectedTab=rankcard";
}
