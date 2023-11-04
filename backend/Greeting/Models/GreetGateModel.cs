using System.ComponentModel.DataAnnotations;

namespace Greeting.Models;

public class GreetGateModel
{
    [Key] public ulong GuildId { get; set; }

    public ulong[] AllowedGreetChannels { get; set; }
    public ulong[] AllowedGreetRoles { get; set; }

    public ulong[] DisallowedMuteRoles { get; set; }
    public TimeSpan DisallowedMuteExistence { get; set; }

    public TimeSpan PunishmentTime { get; set; }

    public ulong LoggingChannel { get; set; }
}
