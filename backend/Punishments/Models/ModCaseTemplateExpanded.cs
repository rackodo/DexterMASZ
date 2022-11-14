using Bot.Models;
using Discord;

namespace Punishments.Models;

public class ModCaseTemplateExpanded
{
    public ModCaseTemplate CaseTemplate { get; set; }
    public DiscordUser Creator { get; set; }
    public DiscordGuild Guild { get; set; }

    public ModCaseTemplateExpanded(ModCaseTemplate template, IUser creator, IGuild guild)
    {
        CaseTemplate = template;
        Creator = DiscordUser.GetDiscordUser(creator);
        Guild = DiscordGuild.GetDiscordGuild(guild);
    }
}