namespace RoleReactions.Models;

public class RoleMenu
{
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public string MenuName { get; set; }
    public Dictionary<string, ulong> Roles { get; set; } 
}
