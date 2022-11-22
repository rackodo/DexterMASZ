using Bot.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bot.DTOs;

public class SettingsAppSettingsForPutDto
{
    [Required(ErrorMessage = "DefaultLanguage field is required")]
    public Language DefaultLanguage { get; set; }

    [RegularExpression(@"https://(.*\.)?discord(app)?.com/api/webhooks/[0-9]+/.+",
        ErrorMessage = "Must be a valid webhook url")]
    public string? AuditLogWebhookUrl { get; set; }
}
