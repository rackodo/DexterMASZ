using Discord;
using MASZ.Bot.Views;
using MASZ.Punishments.Views;

namespace MASZ.Punishments.Models;

public class ModCaseTableEntry
{
	public ModCaseTableEntry(ModCase modCase, IUser moderator, IUser suspect)
	{
		ModCase = new CaseView(modCase);
		Moderator = new DiscordUserView(moderator);
		Suspect = new DiscordUserView(suspect);
	}

	public CaseView ModCase { get; set; }
	public DiscordUserView Moderator { get; set; }
	public DiscordUserView Suspect { get; set; }

	public void RemoveModeratorInfo()
	{
		Moderator = null;
		ModCase.RemoveModeratorInfo();
	}
}