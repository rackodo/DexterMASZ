using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels.DTOs;

public class ExperienceRecordDTO
{
	public ExperienceRecordDTO(long xp, int level, long xpLevel, long xpResidual)
	{
		Xp = xp;
		Level = level;
		XpLevel = xpLevel;
		XPResidual = xpResidual;
	}

	public long Xp { get; set; }
	public int Level { get; set; }
	public long XpLevel { get; set; }
	public long XPResidual { get; set; }
}
