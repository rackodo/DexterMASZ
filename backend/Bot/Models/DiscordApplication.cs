using Bot.Exceptions;
using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordApplication
{
	public static DiscordApplication GetDiscordApplication(IApplication application)
	{
		if (application is null)
			return null;
		else if (application.Id is 0)
			return null;
		else
			return new DiscordApplication(application);
	}

	private DiscordApplication(IApplication application)
	{
		Id = application.Id;
		Name = application.Name;
		Description = application.Description;
		IconUrl = application.IconUrl.GetAnimatedOrDefaultAvatar();
		IconHash = application.IconUrl?.GetAnimatedOrDefaultAvatar().Split('/').Last();
		PrivacyPolicyUrl = application.PrivacyPolicy ?? "";
		TermsOfServiceUrl = application.TermsOfService ?? "";
	}

	public ulong Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public string IconUrl { get; set; }
	public string IconHash { get; set; }
	public string PrivacyPolicyUrl { get; set; }
	public string TermsOfServiceUrl { get; set; }
}