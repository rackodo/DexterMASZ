using Bot.Abstractions;
using Bot.Data;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Humanizer;
using Levels.Data;
using Levels.Enums;
using Levels.Models;

namespace Levels.Commands;

public class Experience : Command<Experience>
{
	public GuildConfigRepository? GuildConfigRepository { get; set; }
	public GuildLevelConfigRepository? GuildLevelConfigRepository { get; set; }
	public GuildUserLevelRepository? GuildUserLevelRepository { get; set; }
	public UserRankcardConfigRepository? UserRankcardConfigRepository { get; set; }
	public SettingsRepository? SettingsRepository { get; set; }
	public DiscordRest? _client { get; set; }

	[SlashCommand("xp", "Display detailed experience information.")]
	public async Task RankCommand(
		[Summary("user", "Target user to get rank from.")] IUser? user = null,
		[Summary("level", "The level you wish to get information about")] int levelTarget = -1,
		[Summary("rank", "The rank you wish to get information about")] IRole roleTarget = null)
	{
		if (Context.Channel is not IGuildChannel)
		{
			await DeclineCommand("This command must be executed in a guild context.");
			return;
		}

		user ??= Context.User;

		var level = await GuildUserLevelRepository!.GetOrCreateLevel(Context.Guild.Id, user.Id);
		var guildlevelconfig = await GuildLevelConfigRepository!.GetOrCreateConfig(Context.Guild.Id);
		var calclevel = new CalculatedGuildUserLevel(level, guildlevelconfig);

		var totalLevel = calclevel.Total.Level;
		if (levelTarget < 0) levelTarget = totalLevel + 1;
		var targetXp = GuildUserLevel.XPFromLevel(levelTarget, guildlevelconfig);

		var roleTargetLevel = 0;
		var roleTargetName = "Unknown";
		var found = false;
		var guildInfo = _client.FetchGuildInfo(guildlevelconfig.Id, Bot.Enums.CacheBehavior.Default);
		if (roleTarget != null)
		{
			var entry = guildlevelconfig.Levels.FirstOrDefault((e) =>
			{
				if (e.Value.Contains(roleTarget.Id))
				{
					found = true;
					roleTargetLevel = e.Key;
					return true;
				}
				return false;
			});

			if (!found)
			{
				await RespondAsync("The role you provided isn't part of the rank system! Ensure you're choosing a leveling role.", ephemeral: true);
				return;
			}

			roleTargetName = roleTarget.Name;
		}
		else
		{
			var maxLevel = 0;
			foreach (var levelRole in guildlevelconfig.Levels)
			{
				if (levelRole.Value.Length == 0) continue;
				var r = guildInfo.GetRole(levelRole.Value.First());
				if (levelRole.Key > totalLevel)
				{
					roleTargetLevel = levelRole.Key;
					roleTargetName = r.Name;
					found = true;
					break;
				}

				if (levelRole.Key > maxLevel) maxLevel = levelRole.Key;
			}

			if (!found)
			{
				roleTargetLevel = maxLevel;
				roleTargetName = guildInfo.GetRole(guildlevelconfig.Levels[maxLevel].First()).Name;
			}
		}

		var roleTargetXp = GuildUserLevel.XPFromLevel(roleTargetLevel, guildlevelconfig);

		var embed = new EmbedBuilder()
			.WithTitle($"{user.Username}#{user.Discriminator}'s XP Summary")
			.WithThumbnailUrl(user.GetAvatarUrl())
			.WithDescription(
				$"{LevelDataExpression(LevelType.Total, calclevel)}\n" +
				$"{LevelDataExpression(LevelType.Text, calclevel)}\n" +
				$"{LevelDataExpression(LevelType.Voice, calclevel)}")
			.AddField($"Until Level {levelTarget}:", LevelTargetExpression(level.TotalXP, targetXp, guildlevelconfig))
			.AddField($"Until {roleTargetName} Rank:", LevelTargetExpression(level.TotalXP, roleTargetXp, guildlevelconfig))
			.WithColor(Color.Blue)
			.Build();

		var guildconfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);
		var ephemeral = !guildconfig.BotChannels.Contains(Context.Channel.Id);
		await RespondAsync("", ephemeral: ephemeral, embed: embed);
	}

	private static string LevelDataExpression(LevelType type, CalculatedGuildUserLevel level)
	{
		var data = type switch
		{
			LevelType.Total => level.Total,
			LevelType.Voice => level.Voice,
			LevelType.Text => level.Text,
			_ => throw new NotImplementedException()
		};

		return $"**{type} Level:** {data.Level} ({data.ResidualXp}/{data.LevelXp} xp, total {data.Xp})";
	}

	private static string LevelTargetExpression(long currentXP, long targetXP, GuildLevelConfig config)
	{
		string textExpr;
		try
		{
			var textTime = TimeSpan.FromMinutes((targetXP - currentXP) / ((config.MinimumTextXpGiven + config.MaximumTextXpGiven) >> 1));
			textExpr = textTime.Humanize(2, minUnit: Humanizer.Localisation.TimeUnit.Minute);
		}
		catch
		{
			textExpr = "a **very** long time";
		}

		string voiceExpr;
		try
		{
			var voiceTime = TimeSpan.FromMinutes((targetXP - currentXP) / ((config.MinimumVoiceXpGiven + config.MaximumVoiceXpGiven) >> 1));
			voiceExpr = voiceTime.Humanize(2, minUnit: Humanizer.Localisation.TimeUnit.Minute);
		}
		catch
		{
			voiceExpr = "a **very** long time";
		}

		if (targetXP > currentXP)
			return $"An average of **{textExpr} through text**, or **{voiceExpr} through voice** " +
				$"({currentXP}/{targetXP} xp, missing {targetXP - currentXP})";
		else return $"Exceeded target by {currentXP - targetXP} experience ({currentXP}/{targetXP}).";
	}
}
