using System.ComponentModel.DataAnnotations;

namespace JoinLeave.Models;

public class JoinLeaveConfig
{
    [Key] public int GuildId { get; set; }

    public bool Enabled { get; set; }

    public string JoinMessage { get; set; }
    public ulong JoinChannelId { get; set; }

    public string LeaveMessage { get; set; }
    public ulong LeaveChannelId { get; set; }
}
