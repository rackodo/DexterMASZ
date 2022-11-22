using Punishments.Enums;
using System.ComponentModel.DataAnnotations;

namespace Punishments.Models;

public class ModCase
{
    [Key] public int Id { get; set; }
    [Required] public int CaseId { get; set; }
    [Required] public ulong GuildId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ulong UserId { get; set; }
    public string Username { get; set; }
    public string Discriminator { get; set; }
    public string Nickname { get; set; }
    public ulong ModId { get; set; }
    public SeverityType Severity { get; set; }
    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime OccurredAt { get; set; }
    public DateTime LastEditedAt { get; set; }
    public ulong LastEditedByModId { get; set; }
    public string[] Labels { get; set; }
    public string Others { get; set; }
    public bool? Valid { get; set; } = true;
    public CaseCreationType CreationType { get; set; }
    public PunishmentType PunishmentType { get; set; }
    public DateTime? PunishedUntil { get; set; }
    public bool PunishmentActive { get; set; }
    public bool AllowComments { get; set; } = true;
    public ulong LockedByUserId { get; set; }
    public DateTime? LockedAt { get; set; }
    public DateTime? MarkedToDeleteAt { get; set; }
    public ulong DeletedByUserId { get; set; }
    public ICollection<ModCaseComment> Comments { get; set; }

    public void RemoveModeratorInfo()
    {
        ModId = default;
        LastEditedByModId = default;
        LockedByUserId = default;
        DeletedByUserId = default;
    }
}
