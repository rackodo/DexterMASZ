namespace Levels.DTOs;

public class ExperienceRecordDto
{
    public long Xp { get; set; }
    public int Level { get; set; }
    public long XpLevel { get; set; }
    public long XpResidual { get; set; }

    public ExperienceRecordDto(long xp, int level, long xpLevel, long xpResidual)
    {
        Xp = xp;
        Level = level;
        XpLevel = xpLevel;
        XpResidual = xpResidual;
    }
}
