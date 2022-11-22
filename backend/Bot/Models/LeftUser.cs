using Discord;
using System.ComponentModel.DataAnnotations;

namespace Bot.Models;

public class LeftUser
{
    [Key] public ulong Id { get; set; }
    public bool IsBot { get; set; }
    public string Username { get; set; }
    public ushort DiscriminatorValue { get; set; }
    public string AvatarId { get; set; }
    public UserProperties? PublicFlags { get; set; }
}
