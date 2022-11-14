using Bot.Enums;
using Punishments.Enums;

namespace Punishments.Models;

public class ModCaseTemplate
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public string TemplateName { get; set; }
    public ulong CreatedForGuildId { get; set; }
    public ViewPermission ViewPermission { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CaseTitle { get; set; }
    public string CaseDescription { get; set; }
    public string[] CaseLabels { get; set; }
    public PunishmentType CasePunishmentType { get; set; }
    public SeverityType CaseSeverityType { get; set; }
    public DateTime? CasePunishedUntil { get; set; }
    public bool HandlePunishment { get; set; }
}