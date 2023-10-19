namespace Levels.DTOs;

public class ExperienceRecordDto(long xp, int level, long xpLevel, long xpResidual)
{
    public long Xp { get; set; } = xp;
    public int Level { get; set; } = level;
    public long XpLevel { get; set; } = xpLevel;
    public long XpResidual { get; set; } = xpResidual;
}
