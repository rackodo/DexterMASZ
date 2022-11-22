using System.ComponentModel.DataAnnotations;

namespace Punishments.Models;

public class ModCaseComment
{
    [Key] public int Id { get; set; }
    public ModCase ModCase { get; set; }
    public string Message { get; set; }
    public ulong UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
