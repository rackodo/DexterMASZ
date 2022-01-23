using MASZ.Bot.Enums;
using System.ComponentModel.DataAnnotations;

namespace MASZ.Bot.DTOs;

public class SettingsAppSettingsForPutDto
{
	[Required(ErrorMessage = "DefaultLanguage field is required")]
	public Language DefaultLanguage { get; set; }
	[RegularExpression("https://discord(app)?.com/api/webhooks/[0-9]+/[A-Za-z0-9]+", ErrorMessage = "Must be a valid url")]
	public string? AuditLogWebhookURL { get; set; }
	[Required(ErrorMessage = "PublicFileMode field is required")]
	public bool PublicFileMode { get; set; }
}