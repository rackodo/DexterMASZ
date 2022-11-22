using Levels.DTOs;

namespace Levels.Models;

public class LevelData : IComparable<LevelData>
{
    private long _levelxp;
    private long _residualxp;

    public int Level { get; private set; }

    public long Xp { get; private set; }

    public long LevelXp => _levelxp;
    public long ResidualXp => _residualxp;

    public LevelData()
    {
    }

    public LevelData(long xp, GuildLevelConfig config)
    {
        Xp = xp;
        Recalculate(config);
    }

    public int CompareTo(LevelData? other)
    {
        if (other is null) return 1;
        return Xp.CompareTo(other.Xp);
    }

    public void AddXp(long increment, GuildLevelConfig config)
    {
        Xp += increment;
        _residualxp += increment;
        if (_residualxp < 0 || _residualxp > _levelxp) Recalculate(config);
    }

    public void RemoveXp(long decrement, GuildLevelConfig config) => AddXp(-decrement, config);

    public void SetXp(long value, GuildLevelConfig config) => AddXp(value - Xp, config);

    public void SetLevel(int value, GuildLevelConfig config)
    {
        Level = value;
        Xp = GuildUserLevel.XpFromLevel(value, config);
        _levelxp = GuildUserLevel.XpFromLevel(value + 1, config) - 1;
        _residualxp = 0;
    }

    private void Recalculate(GuildLevelConfig config) =>
        Level = GuildUserLevel.LevelFromXp(Xp, config, out _residualxp, out _levelxp);

    public ExperienceRecordDto ToDto() => new(Xp, Level, _levelxp, _residualxp);
}
