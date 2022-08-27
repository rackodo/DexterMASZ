using Bot.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bot.Models;

public class AppSettings
{
	[Key] public ulong ClientId { get; set; }
	public string DiscordBotToken { get; set; }
	public string ClientSecret { get; set; }
	public string AbsolutePathToFileUpload { get; set; }
	public string ServiceDomain { get; set; }
	public ulong[] SiteAdmins { get; set; }
	public string AuditLogWebhookUrl { get; set; } = string.Empty;
	public bool CorsEnabled { get; set; } = false;
	public EncryptionType EncryptionType { get; set; } = EncryptionType.HTTP;
	[JsonIgnore] public string Lang { get; set; } = "en";
	public string EmbedTitle { get; set; } = "Dexter - a discord moderation bot";
	public string EmbedContent { get; set; } = "Dexter is a moderation bot. Keep track of all moderation events on your server, search reliably for infractions or setup automod to be one step ahead of rule breakers.";

	public string GetServiceUrl() =>
		$"http{(EncryptionType == EncryptionType.HTTPS ? "s" : "")}://{ServiceDomain}";

	[NotMapped]
	public Language DefaultLanguage
	{
		get
		{
			return Lang switch
			{
				"de" => Language.De,
				"it" => Language.It,
				"fr" => Language.Fr,
				"es" => Language.Es,
				"ru" => Language.Ru,
				_ => Language.En
			};
		}
		set
		{
			Lang = value switch
			{
				Language.De => "de",
				Language.It => "it",
				Language.Fr => "fr",
				Language.Es => "es",
				Language.Ru => "ru",
				_ => "en"
			};
		}
	}

	public string GetEmbedData(string url)
	{
		return
			"<html>" +
				"<head>" +
					"<meta property=\"og:site_name\" content=\"Dexter by FeroxFoxxo\" />" +
					"<meta property=\"og:title\" content=\"" + EmbedTitle + "\" />" +
					"<meta property=\"og:url\" content=\"" + url + "\" />" +
					(string.IsNullOrWhiteSpace(EmbedContent) ? "" : "<meta property=\"og:description\" content=\"" + EmbedContent + "\" />") +
				"</head>" +
			"</html>";
	}
}