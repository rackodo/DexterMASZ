using Bot.Extensions;
using Discord;

namespace Bot.Models;

public class DiscordApplication
{
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public string IconHash { get; set; }
    public string PrivacyPolicyUrl { get; set; }
    public string TermsOfServiceUrl { get; set; }

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

    public static DiscordApplication GetDiscordApplication(IApplication application)
    {
        return application is null ? null : application.Id is 0 ? null : new DiscordApplication(application);
    }
}
