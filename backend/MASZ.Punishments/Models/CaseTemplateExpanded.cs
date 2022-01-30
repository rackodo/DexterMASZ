using Discord;
using MASZ.Bot.Models;

namespace MASZ.Punishments.Models;

public class CaseTemplateExpanded
{
	public CaseTemplateExpanded(CaseTemplate template, IUser creator, IGuild guild)
	{
		CaseTemplate = template;
		Creator = new DiscordUser(creator);
		Guild = new DiscordGuild(guild);
	}

	public CaseTemplate CaseTemplate { get; set; }
	public DiscordUser Creator { get; set; }
	public DiscordGuild Guild { get; set; }
}