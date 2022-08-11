using Bot.Abstractions;
using Bot.Data;
using Discord;
using Discord.Interactions;
using Levels.Data;
using Levels.Models;
using SixLabors.ImageSharp;

namespace Levels.Commands;

public class Rank : Command<Rank>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
	public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
	public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }

	private static string Storage(IUser user, string root) => Path.Combine(root, "Media", "Cache", $"Rankcard{user.Id}.png"); 

	[SlashCommand("rank", "Display your rankcard and experience information.", runMode: RunMode.Async)]
	public async Task RankCommand([Summary("user", "Target user to get rank from.")]IUser? user = null)
	{
		if (Context.Channel is not IGuildChannel)
		{
			await DeclineCommand("This command must be executed in a guild context.");
			return;
		}

		var guildconfig = await GuildConfigRepository!.GetGuildConfig(Context.Guild.Id);
		if (!await EnsureBotChannel(guildconfig))
			return;

		if (user is null)
			user = Context.User;

		var rankcardconfig = UserRankcardConfigRepository!.GetOrDefaultRankcard(user);
		var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
		var guildlevelconfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
		var calclevel = new CalculatedGuildUserLevel(level, guildlevelconfig);

		_ = DeferAsync();

		var appconfig = await SettingsRepository!.GetAppSettings();
		var path = Storage(user, appconfig.AbsolutePathToFileUpload);
		using (var rankcardimg = await Models.Rankcard.RenderRankCard(user, calclevel, rankcardconfig, GuildUserLevelRepository, SettingsRepository))
		{
			await rankcardimg.SaveAsPngAsync(path);
		}
		await FollowupAsync($"Level {calclevel.Total.Level}: {calclevel.Total.Xp} XP");
		await Context.Channel.SendFileAsync(path);
		File.Delete(path);
	}
}
