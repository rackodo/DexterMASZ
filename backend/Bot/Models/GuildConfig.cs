using System.ComponentModel.DataAnnotations;
using Bot.Enums;

namespace Bot.Models;

public class GuildConfig
{
    [Key] public ulong GuildId { get; set; }
    public ulong[] ModRoles { get; set; }
    public ulong[] AdminRoles { get; set; }
    public ulong[] StaffChannels { get; set; }
    public ulong[] BotChannels { get; set; }
    public bool ModNotificationDm { get; set; }
    public ulong StaffLogs { get; set; }
    public ulong StaffAnnouncements { get; set; }
    public bool StrictModPermissionCheck { get; set; }
    public bool ExecuteWhoIsOnJoin { get; set; }
    public bool PublishModeratorInfo { get; set; } = true;
    public Language PreferredLanguage { get; set; }
}