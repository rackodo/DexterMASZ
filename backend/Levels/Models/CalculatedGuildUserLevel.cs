using Bot.Models;
using Levels.DTOs;

namespace Levels.Models;

public class CalculatedGuildUserLevel : GuildUserLevel
{
	private LevelData? _textLevel = null;
	private LevelData? _voiceLevel = null;
	private LevelData? _totalLevel = null;

	public GuildLevelConfig? Config { get; set; } = null;

	public CalculatedGuildUserLevel(GuildUserLevel guildUserLevel, GuildLevelConfig? config = null)
	{
		Token = guildUserLevel.Token;
		UserId = guildUserLevel.UserId;
		GuildId = guildUserLevel.GuildId;
		TextXp = guildUserLevel.TextXp;
		VoiceXp = guildUserLevel.VoiceXp;
		Config = config;
	}

	public LevelData LevelFromXP(long xp)
	{
		if (Config is null)
		{
			throw new NullReferenceException("_config is null. Unable to get level from XP before running SetConfig");
		}
		return new LevelData(xp, Config);
	}

	public long XPFromLevel(int level)
	{
		if (Config is null)
		{
			throw new NullReferenceException("_config is null. Unable to get XP from level before running SetConfig");
		}
		return XPFromLevel(level, Config);
	}

	public new long TextXp
	{
		get
		{
			return base.TextXp;
		}
		set
		{
			_textLevel?.SetXp(value, Config!);
			_totalLevel?.SetXp(value + base.VoiceXp, Config!);
			base.TextXp = value;
		}
	}

	public new long VoiceXp
	{
		get
		{
			return base.VoiceXp;
		}
		set
		{
			_voiceLevel?.SetXp(value, Config!);
			_totalLevel?.SetXp(value + base.TextXp, Config!);
			base.VoiceXp = value;
		}
	}
	
	public LevelData Text
	{
		get
		{
			_textLevel ??= LevelFromXP(TextXp);
			return _textLevel;
		}
	}

	public LevelData Voice
	{
		get
		{
			_voiceLevel ??= LevelFromXP(VoiceXp);
			return _voiceLevel;
		}
	}

	public LevelData Total
	{
		get
		{
			_totalLevel ??= LevelFromXP(TextXp + VoiceXp);
			return _totalLevel;
		}
	}

	public GuildUserLevelDTO ToDTO(DiscordUser user)
	{
		return new GuildUserLevelDTO(GuildId, UserId, Text.toDTO(), Voice.toDTO(), Total.toDTO(), user);
	}
}