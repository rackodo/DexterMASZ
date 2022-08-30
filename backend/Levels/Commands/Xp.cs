using Bot.Abstractions;
using Bot.Data;
using Bot.Services;
using Discord;
using Discord.Interactions;
using Humanizer;
using Levels.Data;
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
		[Summary("rank", "The rank you wish to get information about")] IRole roleTarget = null
		)
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

		int totalLevel = calclevel.Total.Level;
		if (levelTarget < 0) levelTarget = totalLevel + 1;
		long targetXp = GuildUserLevel.XPFromLevel(levelTarget, guildlevelconfig);

		int roleTargetLevel = 0;
		string roleTargetName = "Unknown";
		bool found = false;
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
			int maxLevel = 0;
			foreach (KeyValuePair<int, ulong[]> levelRole in guildlevelconfig.Levels)
			{
				if (levelRole.Value.Length == 0) continue;
				IRole r = guildInfo.GetRole(levelRole.Value.First());
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
				
		long roleTargetXp = GuildUserLevel.XPFromLevel(roleTargetLevel, guildlevelconfig);

		var embed = new EmbedBuilder()
			.WithTitle($"XP Summary for: {user.Id}")
			.WithDescription($"Level {LevelDataExpression(calclevel.Total)}\n" +
			$"Text Level: {LevelDataExpression(calclevel.Text)}\n" +
			$"Voice Level: {LevelDataExpression(calclevel.Voice)}")
			.AddField("Experience", $"{level.TotalXP} ({level.TextXp} from text + {level.VoiceXp} from voice)")
			.AddField($"Till level {levelTarget}:", LevelTargetExpression(level.TotalXP, targetXp, guildlevelconfig))
			.AddField($"Till rank {roleTargetName}:", LevelTargetExpression(level.TotalXP, roleTargetXp, guildlevelconfig))
			.Build();

		var guildconfig = await GuildConfigRepository.GetGuildConfig(Context.Guild.Id);
		bool ephemeral = !guildconfig.BotChannels.Contains(Context.Channel.Id);
		await RespondAsync("", ephemeral: ephemeral, embed: embed);
	}

	private string LevelDataExpression(LevelData data)
	{
		return $"{data.Level} ({data.Residualxp}/{data.Levelxp})";
	}

	private string LevelTargetExpression(long currentXP, long targetXP, GuildLevelConfig config)
	{
		string textExpr;
		try
		{
			TimeSpan textTime = TimeSpan.FromMinutes((targetXP - currentXP) / ((config.MinimumTextXpGiven + config.MaximumTextXpGiven) >> 1));
			textExpr = textTime.Humanize(2, minUnit: Humanizer.Localisation.TimeUnit.Minute);
		}
		catch
		{
			textExpr = "a **very** long time";
		}
		string voiceExpr;
		try
		{
			TimeSpan voiceTime = TimeSpan.FromMinutes((targetXP - currentXP) / ((config.MinimumVoiceXpGiven + config.MaximumVoiceXpGiven) >> 1));
			voiceExpr = voiceTime.Humanize(2, minUnit: Humanizer.Localisation.TimeUnit.Minute);
		}
		catch
		{
			voiceExpr = "a **very** long time";
		}
		if (targetXP > currentXP) return $"{currentXP} out of {targetXP} experience. Missing {targetXP - currentXP}; " +
			$"which can be obtained in an average of {textExpr} through text activity " +
			$"or in an average of {voiceExpr} through voice activity.";
		else return $"{currentXP} out of {targetXP}. Exceeded target by {currentXP - targetXP} experience.";
	}
}
