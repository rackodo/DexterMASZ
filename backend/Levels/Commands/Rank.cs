using Bot.Abstractions;
using Bot.Attributes;
using Bot.Data;
using Bot.Exceptions;
using Discord;
using Discord.Interactions;
using Levels.Data;
using Levels.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace Levels.Commands;

public class Rank : Command<Rank>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
	public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
	public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }

	private static string Storage(IUser user, string root) => Path.Combine(root, "Cache", $"Rankcard{user.Id}.png");

	[SlashCommand("rank", "Display your rankcard and experience information.", runMode: RunMode.Async)]
	[BotChannel]
	public async Task RankCommand([Summary("user", "Target user to get rank from.")] IUser? user = null)
	{
		if (Context.Channel is not IGuildChannel)
			throw new UnauthorizedException("This command must be executed in a guild context.");
		
		await DeferAsync();

		user ??= Context.User;

		var rankCardConfig = UserRankcardConfigRepository!.GetOrDefaultRankcard(user);
		var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
		var guildLevelConfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
		var calcLevel = new CalculatedGuildUserLevel(level, guildLevelConfig);

		var path = "";

		try
		{
			var appconfig = await SettingsRepository!.GetAppSettings();
			path = Storage(user, AppDomain.CurrentDomain.BaseDirectory);

			Directory.CreateDirectory(Path.GetDirectoryName(path));

			using (var rankcardimg = await Models.Rankcard.RenderRankCard(user, calcLevel, rankCardConfig, GuildUserLevelRepository, SettingsRepository))
			{
				await rankcardimg.SaveAsPngAsync(path);
			}
			await FollowupWithFileAsync(path);
		}
		catch (Exception ex)
		{
			await FollowupAsync("There was an error while rendering your rankcard!", new Embed[]
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
