using Microsoft.EntityFrameworkCore;

namespace RoleReactions.Models;

[PrimaryKey(nameof(GuildId), nameof(ChannelId), nameof(MenuName))]
public class RoleMenu
{
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public string MenuName { get; set; }
    public ulong MessageId { get; set; }
    public Dictionary<string, ulong> Roles { get; set; }
    public Dictionary<string, string> Emotes { get; set; }
}
