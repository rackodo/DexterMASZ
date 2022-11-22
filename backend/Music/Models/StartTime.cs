using System.ComponentModel.DataAnnotations;

namespace Music.Models;

public class StartTime
{
    [Key] public ulong Id { get; set; }
    public DateTime RadioStartTime { get; set; }
}
