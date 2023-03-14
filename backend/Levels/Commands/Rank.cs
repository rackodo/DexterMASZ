using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Discord;
using Discord.Interactions;
using Levels.Data;
using Levels.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Levels.Commands;

public class Rank : Command<Rank>
{
    public GuildLevelConfigRepository GuildLevelConfigRepository { get; set; }
    public GuildUserLevelRepository GuildUserLevelRepository { get; set; }
    public UserRankcardConfigRepository UserRankcardConfigRepository { get; set; }
    public SettingsRepository SettingsRepository { get; set; }

    private static string Storage(IUser user, string root) => Path.Combine(root, "Cache", $"Rankcard{user.Id}.png");

    [SlashCommand("rank", "Display your rankcard and experience information.", runMode: RunMode.Async)]
    [BotChannel]
    public async Task RankCommand([Summary("user", "Target user to get rank from.")] IUser user = null)
    {
        await DeferAsync();

        user ??= Context.User;

        var rankCardConfig = UserRankcardConfigRepository!.GetRankcard(user.Id);
        var buttons = new ComponentBuilder();

        if (rankCardConfig == null)
        {
            rankCardConfig = new UserRankcardConfig();
            buttons.WithButton("Customize Card", url: await RankCard.GetRankCard(SettingsRepository),
                style: ButtonStyle.Link);
        }

        var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
        var guildLevelConfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
        var calcLevel = new CalculatedGuildUserLevel(level, guildLevelConfig);

        var path = "";

        try
        {
            path = Storage(user, AppDomain.CurrentDomain.BaseDirectory);

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var rankcardimg = await Rankcard.RenderRankCard(user, calcLevel, rankCardConfig,
                       GuildUserLevelRepository, SettingsRepository))
                await rankcardimg.SaveAsPngAsync(path);


            await FollowupWithFileAsync(path);
        }
        catch (Exception ex)
        {
            await FollowupAsync("There was an error while rendering your rankcard!", new[]
            {
                new EmbedBuilder()
                    .WithTitle("An error has occurred!")
                    .WithDescription("An exception took place while handling rankcard rendering, please consult logs.")
                    .Build()
            });
            Logger.LogError(ex, "Exception took place while handling rankcard rendering.");
        }

        if (File.Exists(path))
            File.Delete(path);
    }
}
