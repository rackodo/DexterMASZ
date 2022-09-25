using Levels.DTOs;

namespace Levels.Models;

public class LevelData : IComparable<LevelData>
{
	private int _level;
	private long _xp;
	private long _levelxp;
	private long _residualxp;

	public int Level => _level;
	public long Xp => _xp;
	public long Levelxp => _levelxp;
	public long Residualxp => _residualxp;

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