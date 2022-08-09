using Bot.Models;
using Discord;
using Levels.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Levels.Models;

public class CalculatedGuildUserLevel : GuildUserLevel
{
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

	public new long TextXp { 
		get
		{
			return base.TextXp;
		} 
		set
		{
			if (_textLevel is not null) _textLevel.SetXp(value, Config!);
			if (_totalLevel is not null) _totalLevel.SetXp(value + base.VoiceXp, Config!);
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
			if (_voiceLevel is not null) _voiceLevel.SetXp(value, Config!);
			if (_totalLevel is not null) _totalLevel.SetXp(value + base.TextXp, Config!);
			base.VoiceXp = value;
		}
	}

	private LevelData? _textLevel = null;
	private LevelData? _voiceLevel = null;
	private LevelData? _totalLevel = null;

	public GuildLevelConfig? Config { get; set; } = null;
	
	public LevelData Text { 
		get
		{
			if (_textLevel is null)
				_textLevel = LevelFromXP(TextXp);
			return _textLevel;
		}
	}

	public LevelData Voice
	{
		get
		{
			if (_voiceLevel is null)
				_voiceLevel = LevelFromXP(VoiceXp);
			return _voiceLevel;
		}
	}

	public LevelData Total
	{
		get
		{
			if (_totalLevel is null)
				_totalLevel = LevelFromXP(TextXp + VoiceXp);
			return _totalLevel;
		}
	}

	public GuildUserLevelDTO ToDTO(DiscordUser user)
	{
		return new GuildUserLevelDTO(GuildId, UserId, Text.toDTO(), Voice.toDTO(), Total.toDTO(), user);
	}
}

public class LevelData : IComparable<LevelData>
{
	public void AddXp(long increment, GuildLevelConfig config)
	{
		_xp += increment;
		_residualxp += increment;
		if (_residualxp < 0 || _residualxp > _levelxp) Recalculate(config);
	}

	public void RemoveXp(long decrement, GuildLevelConfig config)
	{
		AddXp(-decrement, config);
	}

	public void SetXp(long value, GuildLevelConfig config)
	{
		AddXp(value - _xp, config);
	}

	public void SetLevel(int value, GuildLevelConfig config)
	{
		_level = value;
		_xp = GuildUserLevel.XPFromLevel(value, config);
		_levelxp = GuildUserLevel.XPFromLevel(value + 1, config) - 1;
		_residualxp = 0;
	}

	private void Recalculate(GuildLevelConfig config)
	{
		_level = GuildUserLevel.LevelFromXP(_xp, config, out _residualxp, out _levelxp);
	}

	public LevelData() { }
	public LevelData(long xp, GuildLevelConfig config)
	{
		_xp = xp;
		Recalculate(config);
	} 

	private int _level;
	private long _xp;
	private long _levelxp;
	private long _residualxp;

	public int Level => _level;
	public long Xp => _xp;
	public long Levelxp => _levelxp;
	public long Residualxp => _residualxp;

	public ExperienceRecordDTO toDTO()
	{
		return new ExperienceRecordDTO(_xp, _level, _levelxp, _residualxp);
	}

	public int CompareTo(LevelData? other)
	{
		if (other is null) return 1;
		return Xp.CompareTo(other.Xp);
	}
}