using Bot.Models;
using Discord;

namespace Punishments.Models;

public class ModCaseTemplateExpanded(ModCaseTemplate template, IUser creator, IGuild guild)
{
    public ModCaseTemplate CaseTemplate { get; set; } = template;
    public DiscordUser Creator { get; set; } = DiscordUser.GetDiscordUser(creator);
    public DiscordGuild Guild { get; set; } = DiscordGuild.GetDiscordGuild(guild);
}
