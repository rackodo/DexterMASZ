using System.ComponentModel.DataAnnotations;

namespace RoleReactions.Models;

public class AssignedRole
{
    [Key] public string RoleName { get; set; }
    public ulong RoleId { get; set; }
    public string Emote { get; set; }
}
