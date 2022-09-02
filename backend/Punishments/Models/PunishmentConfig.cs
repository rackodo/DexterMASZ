using System.ComponentModel.DataAnnotations;

namespace Punishments.Models;

public class PunishmentConfig
{

	[Key]
	public ulong GuildId { get; set; }

	public TimeSpan FinalWarnMuteTime { get; set; }
	public Dictionary<short, TimeSpan> PointMuteTimes { get; set; } = new();

}
