using Discord;
using MASZ.Bot.Views;
using MASZ.Punishments.Models;

namespace MASZ.Punishments.Views;

public class CaseTemplateExpandedView
{
	public CaseTemplateExpandedView(CaseTemplate template, IUser creator, IGuild guild)
	{
		CaseTemplate = new CaseTemplateView(template);
		Creator = new DiscordUserView(creator);
		Guild = new DiscordGuildView(guild);
	}

	public CaseTemplateView CaseTemplate { get; set; }
	public DiscordUserView Creator { get; set; }
	public DiscordGuildView Guild { get; set; }
}