using Microsoft.EntityFrameworkCore;

namespace RoleReactions.Models;

[PrimaryKey(nameof(GuildId), nameof(UserId))]
public class UserRoles
{
    public ulong GuildId { get; set; }
    public ulong UserId { get; set; }
    public List<ulong> RoleIds { get; set; }
}
